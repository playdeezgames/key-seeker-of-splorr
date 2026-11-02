// Saving. A game in progress is one line of space-separated integers behind a tag. Every field is read back with its
// legal range, the references between things are checked, and a parse only succeeds if the whole line is consumed;
// a failed parse leaves the caller's game untouched. The writer and the reader list the fields in the same order (a
// round-trip test pins them together). Pending messages are not saved: the game is only saved between turns.
package game

import "core:fmt"
import "core:strings"

SAVE_TAG :: "KSOS"
SAVE_VERSION :: 1
BIG :: 100_000_000

put :: proc(sb: ^strings.Builder, v: int) {
	fmt.sbprintf(sb, " %d", v)
}

serialize_game :: proc(g: ^Game) -> string {
	sb := strings.builder_make(context.temp_allocator)
	fmt.sbprintf(&sb, "%s %d", SAVE_TAG, SAVE_VERSION)
	fmt.sbprintf(&sb, " %d", g.rng.state)
	put(&sb, g.avatar)
	put(&sb, g.location_count)
	for l in 0 ..< g.location_count {
		loc := &g.locations[l]
		put(&sb, int(loc.type))
		put(&sb, loc.route_count)
		for k in 0 ..< loc.route_count {
			d := loc.order[k]
			r := loc.routes[d]
			put(&sb, int(d))
			put(&sb, r.to)
			put(&sb, int(r.type))
			put(&sb, int(r.required))
			put(&sb, 1 if r.single_use else 0)
		}
		put(&sb, loc.feature_count)
		for k in 0 ..< loc.feature_count {
			f := &loc.features[k]
			put(&sb, int(f.kind))
			put(&sb, int(f.loot_type))
			put(&sb, f.loot_qty)
			for t in 0 ..< len(shop_trades(f.kind)) {
				put(&sb, f.avail[t])
			}
		}
	}
	put(&sb, g.character_count)
	for c in 0 ..< g.character_count {
		ch := &g.characters[c]
		if !ch.alive {
			put(&sb, 0)
			continue
		}
		put(&sb, 1)
		put(&sb, int(ch.type))
		put(&sb, ch.loc)
		put(&sb, ch.name_len)
		for k in 0 ..< ch.name_len {
			put(&sb, int(ch.name[k]))
		}
		put(&sb, ch.max_health)
		put(&sb, ch.wounds)
		put(&sb, ch.xp)
		for s in Equip_Slot {
			put(&sb, ch.equipment[s] + 1)
		}
	}
	put(&sb, g.next_order)
	count := 0
	for i in 0 ..< MAX_ITEMS {
		if g.items[i].type != .None {
			count += 1
		}
	}
	put(&sb, count)
	for i in 0 ..< MAX_ITEMS {
		it := g.items[i]
		if it.type == .None {
			continue
		}
		put(&sb, i)
		put(&sb, int(it.type))
		put(&sb, it.qty)
		put(&sb, it.wear)
		put(&sb, it.order)
		put(&sb, int(it.place))
		put(&sb, it.owner)
	}
	return strings.to_string(sb)
}

Reader :: struct {
	text: string,
	at:   int,
	ok:   bool,
}

// The next whole number, 0..hi. Anything else (a letter, a minus sign, too big, the end of the text) fails the read.
read :: proc(r: ^Reader, hi: int) -> int {
	for r.at < len(r.text) && r.text[r.at] == ' ' {
		r.at += 1
	}
	start := r.at
	v: u64 = 0
	for r.at < len(r.text) && r.text[r.at] >= '0' && r.text[r.at] <= '9' {
		v = v * 10 + u64(r.text[r.at] - '0')
		if v > 1 << 62 {
			r.ok = false
			return 0
		}
		r.at += 1
	}
	if r.at == start || v > u64(hi) {
		r.ok = false
		return 0
	}
	return int(v)
}

read_u64 :: proc(r: ^Reader) -> u64 {
	for r.at < len(r.text) && r.text[r.at] == ' ' {
		r.at += 1
	}
	start := r.at
	v: u64 = 0
	for r.at < len(r.text) && r.text[r.at] >= '0' && r.text[r.at] <= '9' {
		d := u64(r.text[r.at] - '0')
		if v > (max(u64) - d) / 10 {
			r.ok = false
			return 0
		}
		v = v * 10 + d
		r.at += 1
	}
	if r.at == start {
		r.ok = false
	}
	return v
}

// Reads a saved game into g. g is only touched if the whole text is a good save.
parse_game :: proc(g: ^Game, text: string) -> bool {
	s := new(Game)
	defer free(s)
	r := Reader {
		text = text,
		ok   = true,
	}
	if !strings.has_prefix(text, SAVE_TAG) {
		return false
	}
	r.at = len(SAVE_TAG)
	if read(&r, 1 << 20) != SAVE_VERSION {
		return false
	}
	s.rng.state = read_u64(&r)
	s.avatar = read(&r, MAX_CHARACTERS - 1)
	s.location_count = read(&r, MAX_LOCATIONS)
	if !r.ok || s.location_count < 1 {
		return false
	}
	for l in 0 ..< s.location_count {
		loc := &s.locations[l]
		loc.type = Location_Type(read(&r, len(Location_Type) - 1))
		loc.route_count = read(&r, DIRECTION_COUNT)
		for k in 0 ..< loc.route_count {
			d := Direction(read(&r, DIRECTION_COUNT - 1))
			if loc.routes[d].present {
				return false // the same way out twice
			}
			loc.order[k] = d
			route := Route {
				present = true,
			}
			route.to = read(&r, s.location_count - 1)
			route.type = Route_Type(read(&r, len(Route_Type) - 1))
			route.required = Item_Type(read(&r, len(Item_Type) - 1))
			route.single_use = read(&r, 1) == 1
			loc.routes[d] = route
			if !r.ok {
				return false
			}
		}
		loc.feature_count = read(&r, MAX_LOC_FEATURES)
		for k in 0 ..< loc.feature_count {
			f := &loc.features[k]
			f.kind = Feature_Kind(read(&r, len(Feature_Kind) - 1))
			if f.kind == .None {
				return false
			}
			f.loot_type = Item_Type(read(&r, len(Item_Type) - 1))
			f.loot_qty = read(&r, BIG)
			for t in 0 ..< MAX_TRADES {
				f.avail[t] = UNLIMITED
			}
			for t in 0 ..< len(shop_trades(f.kind)) {
				f.avail[t] = read(&r, UNLIMITED)
			}
			if feature_class(f.kind) == .Loot && f.loot_type == .None {
				return false
			}
		}
		if !r.ok {
			return false
		}
	}
	s.character_count = read(&r, MAX_CHARACTERS)
	for c in 0 ..< s.character_count {
		if read(&r, 1) == 0 {
			continue
		}
		ch := &s.characters[c]
		ch.alive = true
		ch.type = Character_Type(read(&r, len(Character_Type) - 1))
		ch.loc = read(&r, s.location_count - 1)
		ch.name_len = read(&r, NAME_MAX)
		for k in 0 ..< ch.name_len {
			ch.name[k] = u8(read(&r, 255))
		}
		ch.max_health = read(&r, 1000)
		ch.wounds = read(&r, ch.max_health)
		ch.xp = read(&r, BIG)
		for slot in Equip_Slot {
			ch.equipment[slot] = read(&r, MAX_ITEMS) - 1
		}
		if !r.ok || ch.max_health < 1 {
			return false
		}
	}
	s.next_order = read(&r, BIG)
	item_count := read(&r, MAX_ITEMS)
	if !r.ok {
		return false
	}
	for _ in 0 ..< item_count {
		i := read(&r, MAX_ITEMS - 1)
		if s.items[i].type != .None {
			return false // the same slot twice
		}
		it: Item
		it.type = Item_Type(read(&r, len(Item_Type) - 1))
		it.qty = read(&r, BIG)
		it.wear = read(&r, BIG)
		it.order = read(&r, BIG)
		it.place = Place(read(&r, len(Place) - 1))
		it.owner = read(&r, max(MAX_CHARACTERS, MAX_LOCATIONS) - 1)
		if !r.ok || it.type == .None || it.qty < 1 || it.order > s.next_order {
			return false
		}
		if it.wear > item_descs[it.type].durability {
			return false
		}
		switch it.place {
		case .Limbo:
			return false
		case .Carried, .Equipped:
			if it.owner >= s.character_count || !s.characters[it.owner].alive {
				return false
			}
		case .Ground:
			if it.owner >= s.location_count {
				return false
			}
		}
		s.items[i] = it
	}
	if !r.ok {
		return false
	}
	for r.at < len(text) && text[r.at] == ' ' {
		r.at += 1
	}
	if r.at != len(text) {
		return false // something is left over
	}

	// the references between things
	if !s.characters[s.avatar].alive || s.characters[s.avatar].type != .N00b {
		return false
	}
	equipped := 0
	for c in 0 ..< s.character_count {
		ch := &s.characters[c]
		if !ch.alive {
			continue
		}
		for slot in Equip_Slot {
			i := ch.equipment[slot]
			if i < 0 {
				continue
			}
			it := s.items[i]
			want, can := item_descs[it.type].slot.?
			if it.type == .None || it.place != .Equipped || it.owner != c || !can || want != slot {
				return false
			}
			equipped += 1
		}
	}
	for i in 0 ..< MAX_ITEMS {
		if s.items[i].place == .Equipped && s.items[i].type != .None {
			equipped -= 1
		}
	}
	if equipped != 0 {
		return false
	}

	rng_state := s.rng.state
	g^ = s^
	g.rng.state = rng_state
	g.has_world = true
	g.screen = .Navigation
	g.ret = .Navigation
	return true
}
