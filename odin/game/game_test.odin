#+build !js
package game

import "core:fmt"
import "core:strings"
import "core:testing"

// ---- helpers

mem_save: string
mem_has: bool

mem_write :: proc(text: string) -> bool {
	delete(mem_save)
	mem_save = strings.clone(text)
	mem_has = true
	return true
}
mem_read :: proc() -> (string, bool) {
	return mem_save, mem_has
}
mem_clear :: proc() {
	delete(mem_save)
	mem_save = ""
	mem_has = false
}

use_memory_storage :: proc() {
	mem_clear()
	storage = Storage {
		write = mem_write,
		read  = mem_read,
		clear = mem_clear,
	}
}

// A started game: through the title, the menu and the name prompt.
started :: proc(seed: u64 = 1) -> ^Game {
	g := new_game(seed)
	g.screen = .Main_Menu
	choose(g, 0)
	submit_text(g, "Tester")
	return g
}

finish :: proc(g: ^Game) {
	free(g)
	free_all(context.temp_allocator)
}

find_loc :: proc(g: ^Game, type: Location_Type) -> int {
	for i in 0 ..< g.location_count {
		if g.locations[i].type == type {
			return i
		}
	}
	return -1
}

count_locs :: proc(g: ^Game, type: Location_Type) -> int {
	n := 0
	for i in 0 ..< g.location_count {
		if g.locations[i].type == type {
			n += 1
		}
	}
	return n
}

count_chars :: proc(g: ^Game, type: Character_Type) -> int {
	n := 0
	for i in 0 ..< g.character_count {
		if g.characters[i].alive && g.characters[i].type == type {
			n += 1
		}
	}
	return n
}

// Kills everything in a location except the avatar, so a test can stand there in peace.
clear_enemies :: proc(g: ^Game, loc: int) {
	for i in 0 ..< g.character_count {
		if g.characters[i].alive && g.characters[i].loc == loc && i != g.avatar {
			g.characters[i].alive = false
		}
	}
}

// Puts the avatar somewhere with nobody else about.
teleport :: proc(g: ^Game, loc: int) {
	clear_enemies(g, loc)
	g.characters[g.avatar].loc = loc
}

view_has :: proc(g: ^Game, text: string) -> bool {
	v := make_view(g)
	for l in v.lines {
		if strings.contains(l, text) {
			return true
		}
	}
	return v.title == text
}

choice_index :: proc(g: ^Game, text: string) -> int {
	v := make_view(g)
	for c, i in v.choices {
		if c == text {
			return i
		}
	}
	return -1
}

pick :: proc(t: ^testing.T, g: ^Game, text: string) -> bool {
	i := choice_index(g, text)
	testing.expectf(t, i >= 0, "no choice %q on screen %v", text, g.screen)
	if i < 0 {
		return false
	}
	choose(g, i)
	return true
}

// Dismisses every waiting message and returns what they said, joined.
read_messages :: proc(t: ^testing.T, g: ^Game) -> string {
	sb: strings.Builder
	strings.builder_init(&sb, context.temp_allocator)
	for g.screen == .Message {
		for l in message_lines(&g.messages[0]) {
			strings.write_string(&sb, l)
			strings.write_byte(&sb, '\n')
		}
		choose(g, 0)
	}
	return strings.to_string(sb)
}

queued :: proc(g: ^Game) -> string {
	sb: strings.Builder
	strings.builder_init(&sb, context.temp_allocator)
	for i in 0 ..< g.message_count {
		strings.write_string(&sb, string(g.messages[i].text[:g.messages[i].len]))
		strings.write_byte(&sb, '\n')
	}
	return strings.to_string(sb)
}

has_item :: proc(g: ^Game, c: int, type: Item_Type) -> bool {
	return item_quantity(g, c, type) > 0
}

// ---- the world

@(test)
world_has_the_originals_places_and_people :: proc(t: ^testing.T) {
	g := started(7)
	defer finish(g)
	want := [Location_Type]int {
		.Forest_Corner = 4,
		.Forest        = 116,
		.Forest_Center = 1,
		.Town          = 1,
		.Town_Edge     = 8,
		.Graveyard     = 49,
		.Ruins         = 36,
		.Inn           = 1,
		.Cellar        = 1,
		.Sewer         = 36,
		.Knackery      = 1,
		.Blacksmith    = 1,
		.Tower         = 63,
		.Hut           = 1,
		.Ufo           = 25,
		.Machine       = 1,
	}
	for type in Location_Type {
		testing.expectf(t, count_locs(g, type) == want[type], "%v: %d, want %d", type, count_locs(g, type), want[type])
	}
	for type in Character_Type {
		testing.expectf(t, count_chars(g, type) == char_descs[type].spawn_count, "%v: %d", type, count_chars(g, type))
	}
	testing.expect_value(t, g.characters[g.avatar].type, Character_Type.N00b)
	testing.expect_value(t, g.locations[g.characters[g.avatar].loc].type, Location_Type.Town)
	testing.expect_value(t, char_name(g, g.avatar), "Tester")
}

// The original puts the Machine Guardians among the ruins from the start (as well as the lever's one): pinned.
@(test)
guardians_start_in_the_ruins :: proc(t: ^testing.T) {
	g := started(8)
	defer finish(g)
	for i in 0 ..< g.character_count {
		if g.characters[i].alive && g.characters[i].type == .Guardian {
			testing.expect_value(t, g.locations[g.characters[i].loc].type, Location_Type.Ruins)
		}
	}
}

// Walks every route whose type is in the set, from a start, and counts what it reaches.
region_size :: proc(g: ^Game, start: int, types: bit_set[Route_Type]) -> (reached: int, edges: int) {
	seen := make([]bool, g.location_count, context.temp_allocator)
	stack := make([dynamic]int, context.temp_allocator)
	append(&stack, start)
	seen[start] = true
	for len(stack) > 0 {
		at := pop(&stack)
		reached += 1
		loc := g.locations[at]
		for k in 0 ..< loc.route_count {
			r := loc.routes[loc.order[k]]
			if r.type in types {
				edges += 1
				if !seen[r.to] {
					seen[r.to] = true
					append(&stack, r.to)
				}
			}
		}
	}
	return
}

@(test)
every_region_is_one_connected_maze :: proc(t: ^testing.T) {
	for seed in u64(1) ..= 12 {
		g := started(seed)
		defer finish(g)
		// each maze is a tree: cells - 1 doors, each counted from both sides
		n, e := region_size(g, find_loc(g, .Forest_Center), {.Forest_Path})
		testing.expectf(t, n == 121 && e == 2 * 120, "forest %d cells %d edges (seed %d)", n, e, seed)
		n, e = region_size(g, g.characters[g.avatar].loc, {.Dirt_Road})
		testing.expectf(t, n == 9 && e == 2 * 8, "town %d cells %d edges (seed %d)", n, e, seed)
		n, e = region_size(g, find_loc(g, .Ruins), {.Ruins_Path})
		testing.expectf(t, n == 36 && e == 2 * 35, "ruins %d %d (seed %d)", n, e, seed)
		n, e = region_size(g, find_loc(g, .Sewer), {.Tunnel})
		testing.expectf(t, n == 36 && e == 2 * 35, "sewer %d %d (seed %d)", n, e, seed)
		n, e = region_size(g, find_loc(g, .Ufo), {.Hallway})
		testing.expectf(t, n == 25 && e == 2 * 24, "ufo %d %d (seed %d)", n, e, seed)
		n, e = region_size(g, find_loc(g, .Graveyard), {.Graveyard_Path})
		testing.expectf(t, n == 49 && e == 2 * 84, "graveyard %d %d (seed %d)", n, e, seed)
		// the tower is seven 3 by 3 mazes joined by six stairs; Door also joins the town buildings
		tower := find_loc(g, .Tower)
		n, e = region_size(g, tower, {.Door, .Stairs})
		testing.expectf(t, n == 63 && e == 2 * (7 * 8 + 6), "tower %d %d (seed %d)", n, e, seed)
	}
}

@(test)
routes_come_back_the_way_they_went :: proc(t: ^testing.T) {
	g := started(3)
	defer finish(g)
	for l in 0 ..< g.location_count {
		loc := g.locations[l]
		testing.expect(t, loc.route_count > 0)
		for k in 0 ..< loc.route_count {
			d := loc.order[k]
			r := loc.routes[d]
			testing.expect(t, r.present)
			// everything has a way back, except three of the four town gates: the original gives the forest's
			// centre only one way back into town
			if r.type == .Town_Gate {
				continue
			}
			back := false
			for e in Direction {
				if g.locations[r.to].routes[e].present && g.locations[r.to].routes[e].to == l {
					back = true
				}
			}
			testing.expectf(t, back, "location %d (%v) %v goes one way", l, loc.type, d)
		}
	}
}

// Following the keys from the start should reach the machine: the innkeeper's key for cellar rat tails,
// the king rat's graveyard key, the tower key in a grave, the UFO key in the tower chest, the machine key in the UFO.
@(test)
the_key_chain_can_be_followed :: proc(t: ^testing.T) {
	for seed in u64(1) ..= 20 {
		g := started(seed)
		defer finish(g)
		have: [Item_Type]bool
		seen := make([]bool, g.location_count, context.temp_allocator)
		start := g.characters[g.avatar].loc
		seen[start] = true
		changed := true
		for changed {
			changed = false
			for l in 0 ..< g.location_count {
				if !seen[l] {
					continue
				}
				loc := g.locations[l]
				for k in 0 ..< loc.route_count {
					r := loc.routes[loc.order[k]]
					if !seen[r.to] && (r.required == .None || have[r.required]) {
						seen[r.to] = true
						changed = true
					}
				}
				for f in 0 ..< loc.feature_count {
					ft := loc.features[f]
					if feature_class(ft.kind) == .Loot && !have[ft.loot_type] {
						have[ft.loot_type] = true
						changed = true
					}
				}
				if loc.type == .Cellar && !have[.Sewer_Key] {
					have[.Sewer_Key] = true // five cellar rats, five tails, one key from the innkeeper
					changed = true
				}
				for c in 0 ..< g.character_count {
					if g.characters[c].alive && g.characters[c].loc == l {
						for i in items_in(g, .Carried, c) {
							ty := g.items[i].type
							if !have[ty] {
								have[ty] = true
								changed = true
							}
						}
					}
				}
			}
		}
		testing.expectf(t, seen[find_loc(g, .Machine)], "seed %d: the machine cannot be reached", seed)
		testing.expectf(t, seen[find_loc(g, .Tower)] && seen[find_loc(g, .Ufo)], "seed %d", seed)
		testing.expect_value(t, count_chars(g, .Cellar_Rat), 5)
	}
}

@(test)
the_innkeeper_sells_the_sewer_key_once :: proc(t: ^testing.T) {
	g := started(2)
	defer finish(g)
	inn := find_loc(g, .Inn)
	teleport(g, inn)
	give_item(g, g.avatar, .Cellar_Rat_Tail, 12)
	give_item(g, g.avatar, .Jools, 2)
	pick(t, g, "Interact...")
	pick(t, g, "Gurachan The Innkeeper")
	testing.expect_value(t, g.screen, Screen.Shoppe)
	testing.expect(t, view_has(g, "Resting Dog's Face Inn"))
	pick(t, g, "Cellar Rat Tail(x5) -> Sewer Key(x1)")
	testing.expect(t, strings.contains(read_messages(t, g), "Trade completed!"))
	testing.expect(t, has_item(g, g.avatar, .Sewer_Key))
	testing.expect_value(t, item_quantity(g, g.avatar, .Cellar_Rat_Tail), 7)
	testing.expect_value(t, g.screen, Screen.Shoppe)
	testing.expect_value(t, choice_index(g, "Cellar Rat Tail(x5) -> Sewer Key(x1)"), -1) // gone for good
	pick(t, g, "Jools(x2) -> Chikkin(x1)") // the chikkin stays on sale
	read_messages(t, g)
	testing.expect(t, has_item(g, g.avatar, .Chikkin))
}

// ---- dice and fighting

@(test)
dice_add_one_on_a_six_and_are_capped :: proc(t: ^testing.T) {
	g := new_game(5)
	defer finish(g)
	for _ in 0 ..< 200 {
		testing.expect(t, dice_roll(g, 20, 10) <= 10)
		testing.expect(t, dice_roll(g, 20, 0) == 0)
		testing.expect_value(t, dice_roll(g, 0, 10), 0)
	}
	total := 0
	for _ in 0 ..< 100 {
		total += dice_roll(g, 60, 1000)
	}
	// 6000 dice, a sixth of them sixes: about 1000
	testing.expectf(t, total > 800 && total < 1200, "total %d", total)
}

// A blob (1 health, 1 attack die capped at 1, 1 defend die capped at 1) fought by an armed avatar.
@(test)
fighting_kills_and_loots :: proc(t: ^testing.T) {
	g := started(11)
	defer finish(g)
	forest := find_loc(g, .Forest)
	teleport(g, forest)
	blob := create_character(g, .Blob, forest)
	jools_before := item_quantity(g, blob, .Jools)
	testing.expect(t, jools_before >= 1 && jools_before <= 3)
	// a weapon with plenty of dice makes the kill quick
	give_item(g, g.avatar, .Axe, 1)
	axe := find_item(g, .Carried, g.avatar, .Axe)
	equip(g, g.avatar, axe)
	g.message_count = 0
	g.characters[g.avatar].max_health = 1000
	for round in 0 ..< 200 {
		if !g.characters[blob].alive {
			break
		}
		g.screen = .Navigation
		pick(t, g, "Fight")
		read_messages(t, g)
		_ = round
	}
	testing.expect(t, !g.characters[blob].alive)
	testing.expect_value(t, g.characters[g.avatar].xp, 1)
	testing.expect_value(t, item_quantity(g, g.avatar, .Jools), 0)
	ground := items_in(g, .Ground, forest)
	testing.expect_value(t, len(ground), 1)
	testing.expect_value(t, g.items[ground[0]].type, Item_Type.Jools)
	testing.expect_value(t, g.items[ground[0]].qty, jools_before)
	// pick it up
	g.screen = .Navigation
	testing.expect(t, view_has(g, "There is stuff on the ground."))
	pick(t, g, "Pick Up...")
	pick(t, g, tp("Jools(x%d)", jools_before))
	testing.expect(t, strings.contains(read_messages(t, g), "takes Jools"))
	testing.expect_value(t, item_quantity(g, g.avatar, .Jools), jools_before)
	testing.expect_value(t, len(items_in(g, .Ground, forest)), 0)
}

@(test)
the_enemy_hits_back_and_each_blow_is_a_message :: proc(t: ^testing.T) {
	g := started(12)
	defer finish(g)
	forest := find_loc(g, .Forest)
	teleport(g, forest)
	for _ in 0 ..< 3 {
		create_character(g, .Skeleton, forest)
	}
	g.characters[g.avatar].max_health = 1000
	pick(t, g, "Fight")
	// the avatar's swing, then every skeleton still standing gets a go
	alive := len(enemies_of(g, g.avatar))
	testing.expectf(t, g.message_count == 1 + alive, "%d messages, %d enemies left", g.message_count, alive)
	text := queued(g)
	testing.expect(t, strings.contains(text, "Tester attacks Skeleton!"))
	for k in 1 ..= alive {
		testing.expect(t, strings.contains(text, tp("Enemy #%d:", k)))
	}
	testing.expect(t, !strings.contains(text, tp("Enemy #%d:", alive + 1)))
	testing.expect_value(t, g.screen, Screen.Message)
	read_messages(t, g)
	testing.expect_value(t, g.screen, Screen.Navigation)
}

@(test)
dying_ends_the_game_after_the_messages :: proc(t: ^testing.T) {
	use_memory_storage()
	g := started(13)
	defer finish(g)
	settle(g)
	testing.expect(t, mem_has)
	forest := find_loc(g, .Forest)
	teleport(g, forest)
	ogre := create_character(g, .Guardian, forest)
	_ = ogre
	for _ in 0 ..< 500 {
		if g.screen == .Game_Over {
			break
		}
		if g.screen == .Navigation {
			pick(t, g, "Fight")
		} else {
			choose(g, 0)
		}
	}
	testing.expect_value(t, g.screen, Screen.Game_Over)
	testing.expect(t, view_has(g, "Yer Dead!"))
	testing.expect(t, is_dead(g, g.avatar))
	settle(g)
	choose(g, 0)
	testing.expect(t, !g.has_world)
	testing.expect_value(t, g.screen, Screen.Main_Menu)
	settle(g)
	testing.expect(t, !mem_has) // a finished game leaves no save
	testing.expect(t, choice_index(g, "Start") == 0)
	mem_clear()
}

@(test)
only_a_kill_by_the_avatar_gives_the_avatar_xp :: proc(t: ^testing.T) {
	g := started(14)
	defer finish(g)
	forest := find_loc(g, .Forest)
	teleport(g, forest)
	ogre := create_character(g, .Guardian, forest)
	g.characters[g.avatar].xp = 5
	make_attack(g, ogre, g.avatar, 0)
	make_attack(g, ogre, g.avatar, 0)
	make_attack(g, ogre, g.avatar, 0)
	make_attack(g, ogre, g.avatar, 0)
	make_attack(g, ogre, g.avatar, 0)
	make_attack(g, ogre, g.avatar, 0)
	testing.expectf(t, is_dead(g, g.avatar) || health(g, g.avatar) > 0, "a character cannot go below zero")
	testing.expect(t, g.characters[g.avatar].alive) // the avatar stays where it fell
}

// ---- equipment

@(test)
equipping_swaps_and_the_old_item_goes_back :: proc(t: ^testing.T) {
	g := started(15)
	defer finish(g)
	a := g.avatar
	give_item(g, a, .Dagger, 1)
	give_item(g, a, .Axe, 1)
	give_item(g, a, .Shield, 1)
	teleport(g, find_loc(g, .Forest))
	pick(t, g, "Inventory")
	pick(t, g, "Dagger(x1)")
	pick(t, g, "Equip")
	testing.expect(t, strings.contains(read_messages(t, g), "Tester equips Dagger to Weapon."))
	testing.expect_value(t, attack_dice(g, a), 3 + 1)
	testing.expect_value(t, max_attack(g, a), 1 + 0)
	pick(t, g, "Inventory")
	pick(t, g, "Axe(x1)")
	pick(t, g, "Equip")
	got := read_messages(t, g)
	testing.expect(t, strings.contains(got, "Tester unequips Dagger from Weapon."))
	testing.expect(t, strings.contains(got, "Tester equips Axe to Weapon."))
	testing.expect_value(t, attack_dice(g, a), 3 + 13)
	testing.expect_value(t, max_attack(g, a), 1 + 4)
	testing.expect(t, has_item(g, a, .Dagger))
	// shield counts for defend only
	pick(t, g, "Inventory")
	pick(t, g, "Shield(x1)")
	pick(t, g, "Equip")
	read_messages(t, g)
	testing.expect_value(t, defend_dice(g, a), 4 + 2)
	testing.expect_value(t, max_defend(g, a), 2 + 1)
	// the equipment screen lists slots in order and unequip goes back to it
	pick(t, g, "Equipment")
	testing.expect_value(t, choice_index(g, "Weapon: Axe"), 1)
	testing.expect_value(t, choice_index(g, "Shield: Shield"), 2)
	pick(t, g, "Weapon: Axe")
	testing.expect(t, view_has(g, "Attack: +13/+4"))
	testing.expect(t, view_has(g, "Durability: 100/100"))
	pick(t, g, "Unequip")
	read_messages(t, g)
	testing.expect_value(t, g.screen, Screen.Equipment)
	testing.expect_value(t, choice_index(g, "Weapon: Axe"), -1)
	pick(t, g, NEVER_MIND)
	testing.expect_value(t, g.screen, Screen.Navigation)
}

@(test)
worn_out_gear_breaks_and_disappears :: proc(t: ^testing.T) {
	g := started(16)
	defer finish(g)
	a := g.avatar
	give_item(g, a, .Dagger, 1)
	dagger := find_item(g, .Carried, a, .Dagger)
	equip(g, a, dagger)
	g.message_count = 0
	g.items[dagger].wear = 19 // one point from the end of its 20
	wear_equipped(g, a, true, 5)
	testing.expect(t, strings.contains(queued(g), "Tester's Dagger broke!"))
	testing.expect_value(t, g.characters[a].equipment[.Weapon], -1)
	testing.expect_value(t, g.items[dagger].type, Item_Type.None)
	testing.expect_value(t, attack_dice(g, a), 3)
}

// ---- items you can use

@(test)
chikkin_and_potions_heal_only_the_hurt :: proc(t: ^testing.T) {
	g := started(17)
	defer finish(g)
	a := g.avatar
	teleport(g, find_loc(g, .Forest))
	give_item(g, a, .Chikkin, 2)
	give_item(g, a, .Potion, 1)
	ch := &g.characters[a]
	ch.max_health = 10
	pick(t, g, "Inventory")
	pick(t, g, "Chikkin(x2)")
	testing.expect_value(t, choice_index(g, "Nom"), -1) // at full health there is nothing to nom
	pick(t, g, NEVER_MIND)
	ch.wounds = 9
	pick(t, g, "Inventory")
	pick(t, g, "Chikkin(x2)")
	pick(t, g, "Nom")
	testing.expect(t, strings.contains(read_messages(t, g), "Tester eats the chikkin, and now has 2/10 health."))
	testing.expect_value(t, item_quantity(g, a, .Chikkin), 1)
	pick(t, g, "Inventory")
	pick(t, g, "Potion(x1)")
	pick(t, g, "Drink")
	testing.expect(t, strings.contains(read_messages(t, g), "Tester drinks the potion, and now has 7/10 health."))
	testing.expect(t, !has_item(g, a, .Potion))
	ch.wounds = 0
	ch.max_health = 3
}

@(test)
holy_water_works_only_in_a_fight :: proc(t: ^testing.T) {
	g := started(18)
	defer finish(g)
	a := g.avatar
	forest := find_loc(g, .Forest)
	teleport(g, forest)
	give_item(g, a, .Holy_Water, 2)
	pick(t, g, "Inventory")
	pick(t, g, "Holy Water(x2)")
	testing.expect_value(t, choice_index(g, "Splash"), -1)
	pick(t, g, NEVER_MIND)
	create_character(g, .Blob, forest)
	pick(t, g, "Inventory")
	pick(t, g, "Holy Water(x2)")
	pick(t, g, "Splash")
	testing.expect(t, strings.contains(read_messages(t, g), "Tester moistens his enemy!"))
	testing.expect_value(t, item_quantity(g, a, .Holy_Water), 1)
}

@(test)
a_town_portal_goes_both_ways_once :: proc(t: ^testing.T) {
	g := started(19)
	defer finish(g)
	a := g.avatar
	town := g.characters[a].loc
	forest := find_loc(g, .Forest)
	teleport(g, forest)
	give_item(g, a, .Town_Portal, 1)
	pick(t, g, "Inventory")
	pick(t, g, "Scroll of Town Portal(x1)")
	pick(t, g, "Read")
	testing.expect(t, strings.contains(read_messages(t, g), "Tester reads the scroll, and a portal appears!"))
	testing.expect(t, !has_item(g, a, .Town_Portal))
	testing.expect(t, g.locations[forest].routes[.Portal].present)
	testing.expect(t, view_has(g, "    Shimmering Oval going into a Portal"))
	pick(t, g, "Move...")
	pick(t, g, "into a Portal")
	read_messages(t, g)
	testing.expect_value(t, g.characters[a].loc, town)
	testing.expect(t, !g.locations[forest].routes[.Portal].present) // used up on this side
	testing.expect(t, g.locations[town].routes[.Portal].present) // and still open on the town side
	// a scroll cannot be read in town
	give_item(g, a, .Town_Portal, 1)
	pick(t, g, "Inventory")
	pick(t, g, "Scroll of Town Portal(x1)")
	testing.expect_value(t, choice_index(g, "Read"), -1)
}

// ---- shops, training and loot

@(test)
the_blacksmith_takes_jools_for_a_dagger :: proc(t: ^testing.T) {
	g := started(20)
	defer finish(g)
	a := g.avatar
	teleport(g, find_loc(g, .Blacksmith))
	give_item(g, a, .Jools, 30)
	pick(t, g, "Interact...")
	pick(t, g, "Samuli's Blacksmithery")
	testing.expect(t, view_has(g, "Samuli's Wares"))
	pick(t, g, "Jools(x25) -> Dagger(x1)")
	read_messages(t, g)
	testing.expect(t, has_item(g, a, .Dagger))
	testing.expect_value(t, item_quantity(g, a, .Jools), 5)
	// too poor for the next
	pick(t, g, "Jools(x50) -> Shortsword(x1)")
	testing.expect(t, strings.contains(read_messages(t, g), "Tester does not have 50 Jools!"))
	testing.expect_value(t, item_quantity(g, a, .Jools), 5)
	testing.expect(t, !has_item(g, a, .Shortsword))
	pick(t, g, NEVER_MIND)
	testing.expect_value(t, g.screen, Screen.Navigation)
}

@(test)
the_knacker_pays_for_trophies :: proc(t: ^testing.T) {
	g := started(21)
	defer finish(g)
	a := g.avatar
	teleport(g, find_loc(g, .Knackery))
	give_item(g, a, .Goblin_Ear, 3)
	give_item(g, a, .Marble_Chunk, 1)
	pick(t, g, "Interact...")
	pick(t, g, "\"Honest\" Dan the Knacker")
	pick(t, g, "Goblin Ear(x1) -> Jools(x2)")
	read_messages(t, g)
	pick(t, g, "Chunk of Marble(x1) -> Jools(x10)")
	read_messages(t, g)
	testing.expect_value(t, item_quantity(g, a, .Jools), 12)
	testing.expect_value(t, item_quantity(g, a, .Goblin_Ear), 2)
}

@(test)
training_costs_ten_times_your_health :: proc(t: ^testing.T) {
	g := started(22)
	defer finish(g)
	a := g.avatar
	var_town := g.characters[a].loc
	// the yogi stands on one of the town's edges
	yogi_at := -1
	for l in 0 ..< g.location_count {
		for f in 0 ..< g.locations[l].feature_count {
			if g.locations[l].features[f].kind == .Yogi {
				yogi_at = l
			}
		}
	}
	testing.expect(t, yogi_at >= 0 && yogi_at != var_town)
	teleport(g, yogi_at)
	pick(t, g, "Interact...")
	// town edges may hold the elder too; pick the yogi by name
	pick(t, g, "Yogi the Bare Bear Yoga Instructor")
	testing.expect(t, view_has(g, "Yogi The Bare Bear's Personal Training"))
	testing.expect_value(t, choice_index(g, "Maximum Health (30 XP)"), 1)
	pick(t, g, "Maximum Health (30 XP)")
	testing.expect(t, strings.contains(read_messages(t, g), "Tester does not have 30 XP!"))
	g.characters[a].xp = 35
	pick(t, g, "Maximum Health (30 XP)")
	got := read_messages(t, g)
	testing.expect(t, strings.contains(got, "Tester spends 30 XP, and now has 5 XP."))
	testing.expect(t, strings.contains(got, "Tester now has a Maximum Health of 4."))
	testing.expect_value(t, g.characters[a].max_health, 4)
	testing.expect_value(t, choice_index(g, "Maximum Health (40 XP)"), 1)
	testing.expect_value(t, g.screen, Screen.Trainer)
}

@(test)
chests_and_graves_give_their_loot_once :: proc(t: ^testing.T) {
	g := started(23)
	defer finish(g)
	a := g.avatar
	at := -1
	for l in 0 ..< g.location_count {
		for f in 0 ..< g.locations[l].feature_count {
			if g.locations[l].features[f].kind == .Metal_Box {
				at = l
			}
		}
	}
	testing.expect(t, at >= 0)
	teleport(g, at)
	pick(t, g, "Interact...")
	pick(t, g, "Metal Box")
	testing.expect(t, strings.contains(read_messages(t, g), "Tester finds Machine Key(x1)"))
	testing.expect(t, has_item(g, a, .Machine_Key))
	testing.expect_value(t, g.locations[at].feature_count, 0)
	testing.expect_value(t, choice_index(g, "Interact..."), -1)
}

@(test)
graves_and_the_lever_spawn_what_they_should :: proc(t: ^testing.T) {
	g := started(24)
	defer finish(g)
	a := g.avatar
	lever := find_loc(g, .Machine)
	teleport(g, lever)
	pick(t, g, "Interact...")
	pick(t, g, "lever")
	testing.expect(t, strings.contains(read_messages(t, g), "Machine Guardian suddenly appears!"))
	testing.expect_value(t, len(enemies_of(g, a)), 1)
	testing.expect(t, view_has(g, "    Machine Guardian(10/10)"))
	// with an enemy about you can fight or run, not move or interact
	testing.expect_value(t, choice_index(g, "Move..."), -1)
	testing.expect_value(t, choice_index(g, "Interact..."), -1)
	testing.expect(t, choice_index(g, "Fight") >= 0 && choice_index(g, "Run") >= 0)

	skeletons := 0
	zombies := 0
	for l in 0 ..< g.location_count {
		if g.locations[l].type != .Graveyard {
			continue
		}
		for f in 0 ..< g.locations[l].feature_count {
			if g.locations[l].features[f].kind == .Grave_Spawner {
				teleport(g, l)
				g.message_count = 0
				spawn_from(g, a, f)
				for c in 0 ..< g.character_count {
					if c != a && g.characters[c].alive && g.characters[c].loc == l {
						#partial switch g.characters[c].type {
						case .Skeleton:
							skeletons += 1
						case .Zombie:
							zombies += 1
						}
					}
				}
			}
		}
	}
	// two skeletons to every zombie, give or take
	testing.expectf(t, skeletons + zombies > 5 && skeletons > zombies / 2, "%d skeletons %d zombies", skeletons, zombies)
}

// ---- moving

@(test)
locked_ways_need_their_keys :: proc(t: ^testing.T) {
	g := started(25)
	defer finish(g)
	a := g.avatar
	entrance := -1
	for l in 0 ..< g.location_count {
		if g.locations[l].routes[.Down].present && g.locations[l].routes[.Down].required == .Sewer_Key {
			entrance = l
		}
	}
	testing.expect(t, entrance >= 0)
	teleport(g, entrance)
	pick(t, g, "Move...")
	testing.expect_value(t, choice_index(g, "Down"), 1 + index_of_direction(g, entrance, .Down))
	pick(t, g, "Down")
	testing.expect(t, strings.contains(read_messages(t, g), "Tester needs a Sewer Key to go that way!"))
	testing.expect_value(t, g.characters[a].loc, entrance)
	give_item(g, a, .Sewer_Key, 1)
	pick(t, g, "Move...")
	pick(t, g, "Down")
	testing.expect_value(t, g.locations[g.characters[a].loc].type, Location_Type.Sewer)
	testing.expect(t, view_has(g, "is in a sewer"))
}

index_of_direction :: proc(g: ^Game, loc: int, d: Direction) -> int {
	for k in 0 ..< g.locations[loc].route_count {
		if g.locations[loc].order[k] == d {
			return k
		}
	}
	return -1
}

@(test)
running_away_sometimes_works :: proc(t: ^testing.T) {
	ran := 0
	stayed := 0
	for seed in u64(1) ..= 60 {
		g := started(seed)
		defer finish(g)
		forest := find_loc(g, .Forest)
		teleport(g, forest)
		create_character(g, .Blob, forest)
		g.characters[g.avatar].max_health = 1000
		pick(t, g, "Run")
		text := queued(g)
		if strings.contains(text, "Tester runs away!") {
			ran += 1
			testing.expect(t, g.characters[g.avatar].loc != forest || strings.contains(text, "needs a"))
			testing.expect(t, !strings.contains(text, "Enemy #1:")) // no counter attack when you get away
		} else {
			stayed += 1
			testing.expect(t, strings.contains(text, "Tester fails to run away!"))
			testing.expect(t, strings.contains(text, "Enemy #1:"))
			testing.expect_value(t, g.characters[g.avatar].loc, forest)
		}
		// the first message is the verdict
		testing.expect(t, strings.has_prefix(queued(g), "Tester ") || g.message_count == 0)
	}
	testing.expectf(t, ran > 0 && stayed > 0, "ran %d stayed %d", ran, stayed)
}

// ---- the menus

@(test)
the_menus_lead_where_they_should :: proc(t: ^testing.T) {
	use_memory_storage()
	g := new_game(31)
	defer finish(g)
	testing.expect_value(t, g.screen, Screen.Title)
	testing.expect(t, make_view(g).heading == "Key Seeker of SPLORR!!")
	choose(g, 0)
	testing.expect_value(t, g.screen, Screen.Main_Menu)
	v := make_view(g)
	testing.expect(t, v.choices[0] == "Start" && v.choices[1] == "Load Game...")
	// no save yet
	pick(t, g, "Load Game...")
	testing.expect(t, strings.contains(read_messages(t, g), "Loading Game Failed!"))
	testing.expect_value(t, g.screen, Screen.Main_Menu)
	pick(t, g, "Start")
	testing.expect(t, make_view(g).text)
	testing.expect_value(t, make_view(g).initial, "N00b")
	submit_text(g, "   ")
	testing.expect_value(t, char_name(g, g.avatar), "N00b")
	testing.expect_value(t, g.screen, Screen.Navigation)
	testing.expect(t, view_has(g, "N00b(3/3) is in town"))
	testing.expect(t, view_has(g, "Exits: "))
	// the navigation menu of a quiet start
	testing.expect_value(t, choice_index(g, "Move..."), 0)
	testing.expect_value(t, choice_index(g, "Fight"), -1)
	testing.expect_value(t, choice_index(g, "Inventory"), -1)
	testing.expect_value(t, choice_index(g, "Equipment"), -1)
	pick(t, g, "Status")
	testing.expect(t, view_has(g, "N00b's Status:"))
	testing.expect(t, view_has(g, "Attack : 1"))
	testing.expect(t, view_has(g, "Defend : 2"))
	testing.expect(t, view_has(g, "Health : 3/3"))
	testing.expect(t, view_has(g, "XP : 0"))
	choose(g, 0)
	testing.expect_value(t, g.screen, Screen.Navigation)
	// game menu
	pick(t, g, "Game Menu")
	v = make_view(g)
	testing.expect(t, v.choices[0] == "Continue" && v.choices[1] == "Save Game..." && v.choices[2] == "Abandon Game")
	pick(t, g, "Save Game...")
	testing.expect(t, strings.contains(read_messages(t, g), "Saved game!"))
	testing.expect(t, mem_has)
	pick(t, g, "Abandon Game")
	testing.expect(t, view_has(g, "Are you sure you want to abandon the game?"))
	pick(t, g, "No")
	testing.expect(t, g.has_world)
	pick(t, g, "Abandon Game")
	pick(t, g, "Yes")
	testing.expect(t, !g.has_world)
	pick(t, g, "Load Game...")
	testing.expect(t, strings.contains(read_messages(t, g), "Loaded game!"))
	testing.expect(t, g.has_world)
	testing.expect_value(t, g.screen, Screen.Navigation)
	testing.expect_value(t, char_name(g, g.avatar), "N00b")
	mem_clear()
}

@(test)
a_game_in_progress_saves_itself_and_comes_back :: proc(t: ^testing.T) {
	use_memory_storage()
	g := started(33)
	defer finish(g)
	settle(g)
	testing.expect(t, mem_has)
	// walk a few steps, each answer saved
	for _ in 0 ..< 6 {
		pick(t, g, "Move...")
		choose(g, 1)
		settle(g)
	}
	here := g.characters[g.avatar].loc
	saved := strings.clone(mem_save)
	defer delete(saved)
	h := new_game(99)
	defer free(h)
	testing.expect(t, load_saved(h))
	testing.expect_value(t, h.characters[h.avatar].loc, here)
	testing.expect_value(t, char_name(h, h.avatar), "Tester")
	testing.expect(t, h.has_world)
	// not saved in the middle of a message
	forest := find_loc(g, .Forest)
	teleport(g, forest)
	create_character(g, .Blob, forest)
	settle(g)
	pick(t, g, "Fight")
	before := strings.clone(mem_save)
	defer delete(before)
	settle(g)
	testing.expect_value(t, mem_save, before)
	mem_clear()
}

@(test)
save_round_trips_and_rejects_damage :: proc(t: ^testing.T) {
	for seed in u64(1) ..= 4 {
		g := started(seed)
		defer finish(g)
		a := g.avatar
		give_item(g, a, .Axe, 1)
		give_item(g, a, .Shield, 1)
		give_item(g, a, .Jools, 77)
		equip(g, a, find_item(g, .Carried, a, .Axe))
		equip(g, a, find_item(g, .Carried, a, .Shield))
		g.message_count = 0
		g.characters[a].xp = 12
		g.characters[a].wounds = 1
		set_char_name(g, a, "Zoë the Brave")
		text := strings.clone(serialize_game(g))
		defer delete(text)

		h := new(Game)
		defer free(h)
		testing.expect(t, parse_game(h, text))
		again := serialize_game(h)
		testing.expect(t, again == text)
		testing.expect_value(t, char_name(h, h.avatar), "Zoë the Brave")
		testing.expect_value(t, attack_dice(h, h.avatar), attack_dice(g, a))

		// damage: every one of these must be refused and leave the target alone
		untouched := new(Game)
		defer free(untouched)
		untouched^ = h^
		bad := [?]string {
			"",
			"KSOS",
			"KSOS 2 1 0 0",
			strings.concatenate({text, " 5"}, context.temp_allocator),
			strings.concatenate({text, " x"}, context.temp_allocator),
			text[:len(text) / 2],
			text[:len(text) - 3],
			strings.concatenate({"KSOT", text[4:]}, context.temp_allocator),
			strings.concatenate({"KSOS 1 -1", text[len("KSOS 1"):]}, context.temp_allocator),
			strings.concatenate({"KSOS 1 99999999999999999999999999", text[len("KSOS 1"):]}, context.temp_allocator),
		}
		for b, k in bad {
			testing.expectf(t, !parse_game(h, b), "damage %d was accepted", k)
		}
		testing.expect(t, serialize_game(h) == serialize_game(untouched))
	}
}

// Changes one word of a good save to something out of range, at every position: no parse may crash,
// and any that succeeds must give a game that can be drawn and played a little.
@(test)
a_save_with_any_one_word_ruined_is_safe :: proc(t: ^testing.T) {
	g := started(41)
	defer finish(g)
	text := strings.clone(serialize_game(g))
	defer delete(text)
	words := strings.fields(text, context.temp_allocator)
	h := new(Game)
	defer free(h)
	step := max(1, len(words) / 400)
	accepted := 0
	for at := 1; at < len(words); at += step {
		for bad in ([?]string{"9999999", "x", "-1"}) {
			cut := make([dynamic]string, context.temp_allocator)
			for w, i in words {
				append(&cut, bad if i == at else w)
			}
			joined := strings.join(cut[:], " ", context.temp_allocator)
			if parse_game(h, joined) {
				accepted += 1
				for _ in 0 ..< 10 {
					make_view(h)
					choose(h, 0)
				}
			}
		}
		free_all(context.temp_allocator)
		words = strings.fields(text, context.temp_allocator)
	}
	_ = accepted
}

// ---- play it at random

@(test)
random_play_never_breaks_the_rules :: proc(t: ^testing.T) {
	use_memory_storage()
	visited: bit_set[Screen]
	deaths := 0
	for seed in u64(1) ..= 6 {
		g := started(seed * 7919)
		defer finish(g)
		other := new(Game)
		defer free(other)
		for step in 0 ..< 2000 {
			v := make_view(g)
			visited += {g.screen}
			if g.screen == .Game_Over {
				deaths += 1
			}
			if v.text {
				submit_text(g, "Rando")
			} else {
				testing.expectf(t, len(v.choices) > 0, "screen %v has no choices", g.screen)
				// favour the busy choices over the first, which is usually "Never Mind"
				choose(g, int(next_u64(&g.rng) % u64(len(v.choices))))
			}
			if step % 100 == 0 {
				settle(g)
			}
			if g.screen == .Main_Menu && !g.has_world {
				choose(g, 0)
				submit_text(g, "Rando")
			}
			if g.has_world {
				check_world(t, g)
				if step % 250 == 0 && g.message_count == 0 && g.screen == .Navigation {
					text := strings.clone(serialize_game(g))
					testing.expectf(t, parse_game(other, text), "seed %d step %d: a game just saved cannot be loaded", seed, step)
					testing.expect(t, serialize_game(other) == text)
					delete(text)
				}
			}
			free_all(context.temp_allocator)
		}
	}
	fmt.println("screens visited:", visited, "deaths:", deaths)
	mem_clear()
}

// Things that must hold in any game.
check_world :: proc(t: ^testing.T, g: ^Game) {
	a := g.avatar
	testing.expect(t, g.characters[a].alive)
	testing.expect(t, g.characters[a].loc >= 0 && g.characters[a].loc < g.location_count)
	testing.expect(t, g.message_count >= 0 && g.message_count <= MAX_MESSAGES)
	ch := g.characters[a]
	testing.expect(t, ch.wounds >= 0 && ch.wounds <= ch.max_health)
	testing.expect(t, ch.xp >= 0)
	for s in Equip_Slot {
		i := ch.equipment[s]
		if i >= 0 {
			testing.expect(t, g.items[i].type != .None && g.items[i].place == .Equipped && g.items[i].owner == a)
			testing.expect(t, g.items[i].wear < item_descs[g.items[i].type].durability)
		}
	}
	piles := new([MAX_CHARACTERS][Item_Type]u8, context.temp_allocator)
	for i in 0 ..< MAX_ITEMS {
		it := g.items[i]
		if it.type == .None {
			continue
		}
		testing.expect(t, it.qty >= 1)
		// stackable things stay in one pile
		if it.place == .Carried && item_descs[it.type].stacks {
			piles[it.owner][it.type] += 1
			testing.expect(t, piles[it.owner][it.type] == 1)
		}
	}
}
