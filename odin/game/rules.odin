package game

import "core:fmt"
import "core:strings"

// ---- randomness (splitmix64; all modulo work is done in u64 before narrowing, see Gotchas on js_wasm32)

next_u64 :: proc(r: ^Rng) -> u64 {
	r.state += 0x9E3779B97F4A7C15
	z := r.state
	z = (z ~ (z >> 30)) * 0xBF58476D1CE4E5B9
	z = (z ~ (z >> 27)) * 0x94D049BB133111EB
	return z ~ (z >> 31)
}

// Uniform in [0, n).
below :: proc(r: ^Rng, n: int) -> int {
	return int(next_u64(r) % u64(n))
}

// Uniform in [lo, hi], like the original's RNG.FromRange.
range_of :: proc(r: ^Rng, lo, hi: int) -> int {
	return lo + below(r, hi - lo + 1)
}

// The sum of count dice with the given number of sides, like RNG.RollXDY.
roll_xdy :: proc(r: ^Rng, count, sides: int) -> int {
	total := 0
	for _ in 0 ..< count {
		total += range_of(r, 1, sides)
	}
	return total
}

// ---- items

// A new item that is nowhere yet. Returns -1 only if the table is full.
create_item :: proc(g: ^Game, type: Item_Type, qty: int) -> int {
	for i in 0 ..< MAX_ITEMS {
		if g.items[i].type == .None {
			g.items[i] = Item {
				type  = type,
				qty   = qty,
				place = .Limbo,
			}
			return i
		}
	}
	return -1
}

free_item :: proc(g: ^Game, i: int) {
	g.items[i] = {}
}

stacks :: proc(g: ^Game, i: int) -> bool {
	return item_descs[g.items[i].type].stacks
}

item_name :: proc(t: Item_Type) -> string {
	return item_descs[t].name
}

// The index of the first item of this type in a place, or -1.
find_item :: proc(g: ^Game, place: Place, owner: int, type: Item_Type) -> int {
	best := -1
	for i in 0 ..< MAX_ITEMS {
		it := g.items[i]
		if it.type == type && it.place == place && it.owner == owner {
			if best < 0 || it.order < g.items[best].order {
				best = i
			}
		}
	}
	return best
}

// Puts an item into a place. A stackable item joins the stack already there.
put_item :: proc(g: ^Game, i: int, place: Place, owner: int) {
	if stacks(g, i) {
		existing := find_item(g, place, owner, g.items[i].type)
		if existing >= 0 && existing != i {
			g.items[existing].qty += g.items[i].qty
			free_item(g, i)
			return
		}
	}
	g.next_order += 1
	g.items[i].place = place
	g.items[i].owner = owner
	g.items[i].order = g.next_order
}

// The items in a place, oldest first. On the temp allocator.
items_in :: proc(g: ^Game, place: Place, owner: int) -> []int {
	list := make([dynamic]int, context.temp_allocator)
	for i in 0 ..< MAX_ITEMS {
		it := g.items[i]
		if it.type != .None && it.place == place && it.owner == owner {
			append(&list, i)
		}
	}
	// insertion sort by order; the lists are short
	for a in 1 ..< len(list) {
		x := list[a]
		b := a - 1
		for b >= 0 && g.items[list[b]].order > g.items[x].order {
			list[b + 1] = list[b]
			b -= 1
		}
		list[b + 1] = x
	}
	return list[:]
}

item_quantity :: proc(g: ^Game, c: int, type: Item_Type) -> int {
	total := 0
	for i in 0 ..< MAX_ITEMS {
		it := g.items[i]
		if it.type == type && it.place == .Carried && it.owner == c {
			total += it.qty
		}
	}
	return total
}

has_item_quantity :: proc(g: ^Game, c: int, type: Item_Type, qty: int) -> bool {
	return item_quantity(g, c, type) >= qty
}

remove_item_quantity :: proc(g: ^Game, c: int, type: Item_Type, qty: int) {
	left := min(qty, item_quantity(g, c, type))
	for i in items_in(g, .Carried, c) {
		if g.items[i].type != type {
			continue
		}
		if g.items[i].qty >= left {
			g.items[i].qty -= left
			left = 0
		} else {
			left -= g.items[i].qty
			g.items[i].qty = 0
		}
		if g.items[i].qty <= 0 {
			free_item(g, i)
		}
		if left == 0 {
			break
		}
	}
}

give_item :: proc(g: ^Game, c: int, type: Item_Type, qty: int) {
	i := create_item(g, type, qty)
	if i >= 0 {
		put_item(g, i, .Carried, c)
	}
}

drop_item_on_ground :: proc(g: ^Game, loc: int, type: Item_Type, qty: int) {
	i := create_item(g, type, qty)
	if i >= 0 {
		put_item(g, i, .Ground, loc)
	}
}

max_durability :: proc(g: ^Game, i: int) -> int {
	return item_descs[g.items[i].type].durability
}

durability :: proc(g: ^Game, i: int) -> int {
	return clamp(max_durability(g, i) - g.items[i].wear, 0, max_durability(g, i))
}

is_broken :: proc(g: ^Game, i: int) -> bool {
	return durability(g, i) == 0
}

// ---- characters

char_name :: proc(g: ^Game, c: int) -> string {
	ch := &g.characters[c]
	if ch.name_len > 0 {
		return string(ch.name[:ch.name_len])
	}
	return char_descs[ch.type].name
}

set_char_name :: proc(g: ^Game, c: int, name: string) {
	ch := &g.characters[c]
	n := min(len(name), NAME_MAX)
	// do not cut a multi-byte character in half
	for n > 0 && n < len(name) && (name[n] & 0xC0) == 0x80 {
		n -= 1
	}
	copy(ch.name[:], name[:n])
	ch.name_len = n
}

create_character :: proc(g: ^Game, type: Character_Type, loc: int) -> int {
	for i in 0 ..< MAX_CHARACTERS {
		if !g.characters[i].alive {
			d := char_descs[type]
			g.characters[i] = Character {
				alive      = true,
				type       = type,
				loc        = loc,
				max_health = d.max_health,
				xp         = d.xp,
			}
			for s in Equip_Slot {
				g.characters[i].equipment[s] = -1
			}
			if i >= g.character_count {
				g.character_count = i + 1
			}
			provision(g, i)
			return i
		}
	}
	return -1
}

// What a character starts with.
provision :: proc(g: ^Game, c: int) {
	switch g.characters[c].type {
	case .N00b:
	case .Blob:
		give_item(g, c, .Jools, roll_xdy(&g.rng, 1, 3))
	case .Cellar_Rat:
		give_item(g, c, .Cellar_Rat_Tail, 1)
	case .Sewer_Rat:
		give_item(g, c, .Sewer_Rat_Tail, 1)
	case .King_Rat:
		give_item(g, c, .Sewer_Rat_Tail, 1)
		give_item(g, c, .Graveyard_Key, 1)
	case .Mummy:
		give_item(g, c, .Jools, roll_xdy(&g.rng, 4, 6))
	case .Gargoyle:
		give_item(g, c, .Marble_Chunk, roll_xdy(&g.rng, 2, 3))
	case .Guardian:
	case .Moon_Person:
		give_item(g, c, .Empty_Bottle, 1)
	case .Skeleton:
		give_item(g, c, .Skull_Fragment, 1)
	case .Zombie:
		give_item(g, c, .Zombie_Taint, 1)
		if range_of(&g.rng, 1, 100) < 10 {
			give_item(g, c, .Feather, 1)
		}
	case .Goblin:
		give_item(g, c, .Goblin_Ear, 1)
		give_item(g, c, .Jools, roll_xdy(&g.rng, 1, 4))
	case .Orc:
		give_item(g, c, .Orc_Tooth, 1)
		give_item(g, c, .Jools, roll_xdy(&g.rng, 2, 4))
	case .Cyclops:
		give_item(g, c, .Cyclops_Eyeball, 1)
		give_item(g, c, .Jools, roll_xdy(&g.rng, 3, 4))
	}
}

health :: proc(g: ^Game, c: int) -> int {
	ch := g.characters[c]
	return clamp(ch.max_health - ch.wounds, 0, ch.max_health)
}

set_health :: proc(g: ^Game, c: int, value: int) {
	ch := &g.characters[c]
	ch.wounds = clamp(ch.max_health - value, 0, ch.max_health)
}

is_dead :: proc(g: ^Game, c: int) -> bool {
	return health(g, c) == 0
}

take_damage :: proc(g: ^Game, c: int, damage: int) {
	ch := &g.characters[c]
	ch.wounds = clamp(ch.wounds + damage, 0, ch.max_health)
}

// The living characters in a location other than c, in the order they arrived. On the temp allocator.
enemies_of :: proc(g: ^Game, c: int) -> []int {
	list := make([dynamic]int, context.temp_allocator)
	loc := g.characters[c].loc
	for i in 0 ..< g.character_count {
		if i != c && g.characters[i].alive && g.characters[i].loc == loc {
			append(&list, i)
		}
	}
	return list[:]
}

can_fight :: proc(g: ^Game, c: int) -> bool {
	return len(enemies_of(g, c)) > 0
}

has_routes :: proc(g: ^Game, c: int) -> bool {
	return g.locations[g.characters[c].loc].route_count > 0
}

can_move :: proc(g: ^Game, c: int) -> bool {
	return has_routes(g, c) && !can_fight(g, c)
}

can_run :: proc(g: ^Game, c: int) -> bool {
	return has_routes(g, c) && can_fight(g, c)
}

can_pick_up :: proc(g: ^Game, c: int) -> bool {
	return !can_fight(g, c) && len(items_in(g, .Ground, g.characters[c].loc)) > 0
}

can_interact :: proc(g: ^Game, c: int) -> bool {
	return g.locations[g.characters[c].loc].feature_count > 0 && !can_fight(g, c)
}

has_any_equipment :: proc(g: ^Game, c: int) -> bool {
	for s in Equip_Slot {
		if g.characters[c].equipment[s] >= 0 {
			return true
		}
	}
	return false
}

// Attack and defend are numbers of dice; the maximums cap what they can roll.
attack_dice :: proc(g: ^Game, c: int) -> int {
	total := char_descs[g.characters[c].type].attack
	for s in Equip_Slot {
		i := g.characters[c].equipment[s]
		if i >= 0 && is_weapon_type(g.items[i].type) {
			total += item_descs[g.items[i].type].attack
		}
	}
	return total
}

max_attack :: proc(g: ^Game, c: int) -> int {
	total := char_descs[g.characters[c].type].max_attack
	for s in Equip_Slot {
		i := g.characters[c].equipment[s]
		if i >= 0 && is_weapon_type(g.items[i].type) {
			total += item_descs[g.items[i].type].max_attack
		}
	}
	return total
}

defend_dice :: proc(g: ^Game, c: int) -> int {
	total := char_descs[g.characters[c].type].defend
	for s in Equip_Slot {
		i := g.characters[c].equipment[s]
		if i >= 0 && is_armor_type(g.items[i].type) {
			total += item_descs[g.items[i].type].defend
		}
	}
	return total
}

max_defend :: proc(g: ^Game, c: int) -> int {
	total := char_descs[g.characters[c].type].max_defend
	for s in Equip_Slot {
		i := g.characters[c].equipment[s]
		if i >= 0 && is_armor_type(g.items[i].type) {
			total += item_descs[g.items[i].type].max_defend
		}
	}
	return total
}

// Each die is a "1d6/6": it adds one only when it shows a six. The total is capped at the maximum.
dice_roll :: proc(g: ^Game, dice, cap: int) -> int {
	roll := 0
	for _ in 0 ..< dice {
		roll += range_of(&g.rng, 1, 6) / 6
	}
	return clamp(roll, 0, cap)
}

roll_attack :: proc(g: ^Game, c: int) -> int {
	return dice_roll(g, attack_dice(g, c), max_attack(g, c))
}

roll_defend :: proc(g: ^Game, c: int) -> int {
	return dice_roll(g, defend_dice(g, c), max_defend(g, c))
}

// ---- messages: only the avatar keeps them, and the menus show them one at a time

notify :: proc(g: ^Game, who: int, lines: ..string) {
	if !g.has_world || who != g.avatar {
		return
	}
	push_message(g, ..lines)
}

push_message :: proc(g: ^Game, lines: ..string) {
	if g.message_count >= MAX_MESSAGES {
		return
	}
	m := &g.messages[g.message_count]
	m^ = {}
	for line, n in lines {
		if n > 0 && m.len < MESSAGE_BYTES {
			m.text[m.len] = '\n'
			m.len += 1
		}
		room := MESSAGE_BYTES - m.len
		take := min(len(line), room)
		copy(m.text[m.len:], line[:take])
		m.len += take
	}
	g.message_count += 1
}

// For a message that must come before the ones already waiting.
push_message_first :: proc(g: ^Game, line: string) {
	push_message(g, line)
	if g.message_count < 2 {
		return
	}
	last := g.message_count - 1
	first := g.messages[last]
	for i := last; i > 0; i -= 1 {
		g.messages[i] = g.messages[i - 1]
	}
	g.messages[0] = first
}

pop_message :: proc(g: ^Game) {
	if g.message_count == 0 {
		return
	}
	for i in 1 ..< g.message_count {
		g.messages[i - 1] = g.messages[i]
	}
	g.message_count -= 1
}

message_lines :: proc(m: ^Message) -> []string {
	parts := strings.split(string(m.text[:m.len]), "\n", context.temp_allocator)
	return parts
}

tp :: fmt.tprintf

// ---- combat

wear_equipped :: proc(g: ^Game, c: int, weapons: bool, wear: int) {
	left := wear
	for left > 0 {
		candidates := make([dynamic]int, context.temp_allocator)
		for s in Equip_Slot {
			i := g.characters[c].equipment[s]
			if i >= 0 && !is_broken(g, i) && (is_weapon_type(g.items[i].type) == weapons) && (weapons || is_armor_type(g.items[i].type)) {
				append(&candidates, i)
			}
		}
		if len(candidates) == 0 {
			break
		}
		pick := candidates[below(&g.rng, len(candidates))]
		it := &g.items[pick]
		it.wear = clamp(it.wear + 1, 0, max_durability(g, pick))
		left -= 1
		if is_broken(g, pick) {
			notify(g, c, tp("%s's %s broke!", char_name(g, c), item_name(it.type)))
		}
	}
	// a broken item falls off
	for s in Equip_Slot {
		i := g.characters[c].equipment[s]
		if i >= 0 && is_broken(g, i) {
			g.characters[c].equipment[s] = -1
			free_item(g, i)
		}
	}
}

// One attack. index is 0 for the avatar's own swing and 1, 2, ... for the counter attacks.
make_attack :: proc(g: ^Game, a, d: int, index: int) {
	attack_roll := roll_attack(g, a)
	defend_roll := roll_defend(g, d)
	wear_equipped(g, a, true, attack_roll)
	wear_equipped(g, d, false, defend_roll)
	lines := make([dynamic]string, context.temp_allocator)
	an := char_name(g, a)
	dn := char_name(g, d)
	if index > 0 {
		append(&lines, tp("Enemy #%d:", index))
	}
	append(&lines, tp("%s attacks %s!", an, dn))
	append(&lines, tp("%s rolls an attack of %d.", an, attack_roll))
	append(&lines, tp("%s rolls a defend of %d.", dn, defend_roll))
	if attack_roll > defend_roll {
		damage := attack_roll - defend_roll
		append(&lines, tp("%s hits %s for %d damage.", an, dn, damage))
		take_damage(g, d, damage)
		if is_dead(g, d) {
			append(&lines, tp("%s %s %s!", an, char_descs[g.characters[a].type].kill_verb, dn))
			xp := g.characters[d].xp
			if xp > 0 {
				g.characters[a].xp += xp
				append(&lines, tp("%s gains %d XP and now has %d XP!", an, xp, g.characters[a].xp))
			}
			kill(g, d)
		} else {
			append(&lines, tp("%s has %d health left.", dn, health(g, d)))
		}
	} else {
		append(&lines, tp("%s misses.", an))
	}
	notify(g, a, ..lines[:])
	notify(g, d, ..lines[:])
}

// A dead character drops what it carries and leaves. The avatar stays where it fell.
kill :: proc(g: ^Game, c: int) {
	if g.has_world && c == g.avatar {
		return
	}
	loc := g.characters[c].loc
	for i in items_in(g, .Carried, c) {
		put_item(g, i, .Ground, loc)
	}
	g.characters[c].alive = false
}

attack_once :: proc(g: ^Game, a, d, index: int) {
	if is_dead(g, a) || is_dead(g, d) {
		return
	}
	make_attack(g, a, d, index)
}

// Everyone else here hits back, in turn.
counter_attacks :: proc(g: ^Game) {
	index := 1
	for e in enemies_of(g, g.avatar) {
		attack_once(g, e, g.avatar, index)
		index += 1
	}
}

fight :: proc(g: ^Game) {
	enemies := enemies_of(g, g.avatar)
	if len(enemies) == 0 {
		return
	}
	attack_once(g, g.avatar, enemies[0], 0)
	counter_attacks(g)
}

// ---- movement

// Goes through the exit in a direction, if it is there and the avatar has what it needs.
move :: proc(g: ^Game, c: int, d: Direction) {
	loc := g.characters[c].loc
	route := g.locations[loc].routes[d]
	if !route.present {
		notify(g, c, tp("%s cannot go that way!", char_name(g, c)))
		return
	}
	if route.required != .None && !has_item_quantity(g, c, route.required, 1) {
		notify(g, c, tp("%s needs a %s to go that way!", char_name(g, c), item_name(route.required)))
		return
	}
	g.characters[c].loc = route.to
	if route.single_use {
		remove_route(g, loc, d)
	}
}

// Tries a random way out. Not every way picked is a way there is, so it can fail.
run_away :: proc(g: ^Game, c: int) -> bool {
	loc := g.characters[c].loc
	exits: [DIRECTION_COUNT]Direction
	n := 0
	for d in Direction {
		if d == .North || d == .East || d == .South || d == .West || g.locations[loc].routes[d].present {
			exits[n] = d
			n += 1
		}
	}
	chosen := exits[below(&g.rng, n)]
	if g.locations[loc].routes[chosen].present {
		move(g, c, chosen)
		return true
	}
	return false
}

// ---- equipment

equip :: proc(g: ^Game, c: int, i: int) {
	name := char_name(g, c)
	slot, can := item_descs[g.items[i].type].slot.?
	if g.items[i].place != .Carried || g.items[i].owner != c || !can {
		notify(g, c, tp("%s cannot equip that %s.", name, item_name(g.items[i].type)))
		return
	}
	if g.characters[c].equipment[slot] >= 0 {
		unequip(g, c, slot)
	}
	g.items[i].place = .Equipped
	g.items[i].owner = c
	g.characters[c].equipment[slot] = i
	notify(g, c, tp("%s equips %s to %s.", name, item_name(g.items[i].type), equip_slot_name(slot)))
}

unequip :: proc(g: ^Game, c: int, slot: Equip_Slot) {
	name := char_name(g, c)
	i := g.characters[c].equipment[slot]
	if i < 0 {
		notify(g, c, tp("%s has nothing equipped for %s.", name, equip_slot_name(slot)))
		return
	}
	notify(g, c, tp("%s unequips %s from %s.", name, item_name(g.items[i].type), equip_slot_name(slot)))
	g.characters[c].equipment[slot] = -1
	put_item(g, i, .Carried, c)
}

// ---- using things

can_use :: proc(g: ^Game, c: int, i: int) -> bool {
	it := g.items[i]
	if it.place != .Carried || it.owner != c {
		return false
	}
	#partial switch it.type {
	case .Chikkin, .Potion:
		return health(g, c) < g.characters[c].max_health
	case .Town_Portal:
		return g.locations[g.characters[c].loc].type != .Town
	case .Holy_Water:
		return can_fight(g, c)
	}
	return false
}

use_item :: proc(g: ^Game, c: int, i: int) {
	name := char_name(g, c)
	if !can_use(g, c, i) {
		notify(g, c, tp("%s cannot use %s at this time!", name, item_name(g.items[i].type)))
		return
	}
	#partial switch g.items[i].type {
	case .Chikkin:
		set_health(g, c, health(g, c) + 1)
		notify(g, c, tp("%s eats the chikkin, and now has %d/%d health.", name, health(g, c), g.characters[c].max_health))
	case .Potion:
		set_health(g, c, health(g, c) + 5)
		notify(g, c, tp("%s drinks the potion, and now has %d/%d health.", name, health(g, c), g.characters[c].max_health))
	case .Town_Portal:
		town := -1
		for l in 0 ..< g.location_count {
			if g.locations[l].type == .Town {
				town = l
				break
			}
		}
		here := g.characters[c].loc
		add_route(g, here, .Portal, .Portal, town).single_use = true
		add_route(g, town, .Portal, .Portal, here).single_use = true
		notify(g, c, tp("%s reads the scroll, and a portal appears!", name))
	case .Holy_Water:
		notify(g, c, tp("%s moistens his enemy!", name))
	}
	g.items[i].qty -= 1
	if g.items[i].qty <= 0 {
		free_item(g, i)
	}
}

// ---- features

// Does one trade in a shop. Returns false when the avatar cannot pay.
do_trade :: proc(g: ^Game, c: int, f: ^Feature, t: int) -> bool {
	trade := shop_trades(f.kind)[t]
	if !has_item_quantity(g, c, trade.from, trade.from_qty) {
		notify(g, c, tp("%s does not have %d %s!", char_name(g, c), trade.from_qty, item_name(trade.from)))
		return false
	}
	remove_item_quantity(g, c, trade.from, trade.from_qty)
	give_item(g, c, trade.to, trade.to_qty)
	if f.avail[t] < UNLIMITED && f.avail[t] > 0 {
		f.avail[t] -= 1
	}
	notify(g, c, "Trade completed!")
	return true
}

// The trades still on offer, as indexes into the shop's table.
open_trades :: proc(f: ^Feature) -> []int {
	list := make([dynamic]int, context.temp_allocator)
	for _, t in shop_trades(f.kind) {
		if f.avail[t] != 0 {
			append(&list, t)
		}
	}
	return list[:]
}

train_cost :: proc(g: ^Game, c: int) -> int {
	return YOGI_HEALTH_MULTIPLIER * g.characters[c].max_health
}

train :: proc(g: ^Game, c: int) {
	name := char_name(g, c)
	cost := train_cost(g, c)
	if g.characters[c].xp < cost {
		notify(g, c, tp("%s does not have %d XP!", name, cost))
		return
	}
	g.characters[c].xp -= cost
	g.characters[c].max_health += 1
	notify(
		g,
		c,
		tp("%s spends %d XP, and now has %d XP.", name, cost, g.characters[c].xp),
		tp("%s now has a Maximum Health of %d.", name, g.characters[c].max_health),
	)
}

loot :: proc(g: ^Game, c: int, slot: int) {
	loc := g.characters[c].loc
	f := g.locations[loc].features[slot]
	i := create_item(g, f.loot_type, f.loot_qty)
	if i >= 0 {
		notify(g, c, tp("%s finds %s(x%d)", char_name(g, c), item_name(f.loot_type), f.loot_qty))
		put_item(g, i, .Carried, c)
	}
	remove_feature(g, loc, slot)
}

spawn_from :: proc(g: ^Game, c: int, slot: int) {
	loc := g.characters[c].loc
	kind := g.locations[loc].features[slot].kind
	type: Character_Type
	if kind == .Grave_Spawner {
		type = .Skeleton if below(&g.rng, 3) < 2 else .Zombie
	} else {
		type = .Guardian
	}
	enemy := create_character(g, type, loc)
	if enemy >= 0 {
		notify(g, c, tp("%s suddenly appears!", char_name(g, enemy)))
	}
	remove_feature(g, loc, slot)
}
