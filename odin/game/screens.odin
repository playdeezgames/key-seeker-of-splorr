// The menus. The original was a stack of Spectre.Console selection prompts; here each prompt is a Screen, `make_view`
// describes the current one (text, a question, the choices) and `choose` / `submit_text` act on the player's answer.
package game

import "core:strings"

View :: struct {
	heading: string, // the big title on the title screen
	lines:   []string, // the text above the question
	title:   string, // the question
	choices: []string,
	text:    bool, // the answer is typed text rather than a choice
	initial: string, // what the text box starts with
}

NEVER_MIND :: "Never Mind"

// Where the game keeps its one save. The page gives these real browser storage; tests give them a variable.
Storage :: struct {
	write: proc(text: string) -> bool,
	read:  proc() -> (text: string, ok: bool),
	clear: proc(),
}
storage: Storage

Nav :: enum u8 {
	Move,
	Fight,
	Run,
	Pick_Up,
	Inventory,
	Equipment,
	Interact,
	Status,
	Game_Menu,
}

nav_text :: proc(n: Nav) -> string {
	switch n {
	case .Move:
		return "Move..."
	case .Fight:
		return "Fight"
	case .Run:
		return "Run"
	case .Pick_Up:
		return "Pick Up..."
	case .Inventory:
		return "Inventory"
	case .Equipment:
		return "Equipment"
	case .Interact:
		return "Interact..."
	case .Status:
		return "Status"
	case .Game_Menu:
		return "Game Menu"
	}
	return ""
}

// What the avatar can do right now, in the order the original listed it.
nav_actions :: proc(g: ^Game) -> []Nav {
	list := make([dynamic]Nav, context.temp_allocator)
	a := g.avatar
	if can_move(g, a) {
		append(&list, Nav.Move)
	}
	if can_fight(g, a) {
		append(&list, Nav.Fight)
	}
	if can_run(g, a) {
		append(&list, Nav.Run)
	}
	if can_pick_up(g, a) {
		append(&list, Nav.Pick_Up)
	}
	if len(items_in(g, .Carried, a)) > 0 {
		append(&list, Nav.Inventory)
	}
	if has_any_equipment(g, a) {
		append(&list, Nav.Equipment)
	}
	if can_interact(g, a) {
		append(&list, Nav.Interact)
	}
	append(&list, Nav.Status)
	append(&list, Nav.Game_Menu)
	return list[:]
}

item_label :: proc(g: ^Game, i: int) -> string {
	return tp("%s(x%d)", item_name(g.items[i].type), g.items[i].qty)
}

// ---- moving between screens

// Goes to a screen, unless it is the navigation screen and the avatar has died.
arrive :: proc(g: ^Game, s: Screen) {
	if s == .Navigation && is_dead(g, g.avatar) {
		g.screen = .Game_Over
	} else {
		g.screen = s
	}
}

// After an action: any messages it left are shown first, then the game goes on to `ret`.
after :: proc(g: ^Game, ret: Screen) {
	g.ret = ret
	if g.message_count > 0 {
		g.screen = .Message
	} else {
		arrive(g, ret)
	}
}

// ---- what the player sees

make_view :: proc(g: ^Game) -> View {
	v: View
	lines := make([dynamic]string, context.temp_allocator)
	choices := make([dynamic]string, context.temp_allocator)
	a := g.avatar
	switch g.screen {
	case .Title:
		v.heading = "Key Seeker of SPLORR!!"
		append(&choices, "Ok")
	case .Main_Menu:
		v.title = "Main Menu:"
		if g.has_world {
			append(&choices, "Continue", "Save Game...", "Abandon Game")
		} else {
			append(&choices, "Start", "Load Game...")
		}
	case .Name_Prompt:
		v.title = "Character Name?"
		v.text = true
		v.initial = "N00b"
	case .Confirm_Abandon:
		v.title = "Are you sure you want to abandon the game?"
		append(&choices, "No", "Yes")
	case .Navigation:
		loc := g.locations[g.characters[a].loc]
		append(&lines, tp("%s(%d/%d) is in %s", char_name(g, a), health(g, a), g.characters[a].max_health, location_type_name(loc.type)))
		if can_pick_up(g, a) {
			append(&lines, "There is stuff on the ground.")
		}
		enemies := enemies_of(g, a)
		if len(enemies) > 0 {
			append(&lines, "Enemies: ")
			for e in enemies {
				append(&lines, tp("    %s(%d/%d)", char_name(g, e), health(g, e), g.characters[e].max_health))
			}
		}
		if loc.feature_count > 0 {
			append(&lines, "Features:")
			for k in 0 ..< loc.feature_count {
				append(&lines, tp("    %s", feature_name(loc.features[k].kind)))
			}
		}
		append(&lines, "Exits: ")
		for k in 0 ..< loc.route_count {
			d := loc.order[k]
			append(&lines, tp("    %s going %s", route_type_name(loc.routes[d].type), direction_name(d)))
		}
		v.title = "Now What?"
		for n in nav_actions(g) {
			append(&choices, nav_text(n))
		}
	case .Message:
		if g.message_count > 0 {
			for line in message_lines(&g.messages[0]) {
				append(&lines, line)
			}
		}
		append(&choices, "Ok")
	case .Game_Over:
		append(&lines, "Yer Dead!")
		append(&choices, "Ok")
	case .Move:
		v.title = "Move Where?"
		append(&choices, NEVER_MIND)
		loc := g.locations[g.characters[a].loc]
		for k in 0 ..< loc.route_count {
			append(&choices, direction_name(loc.order[k]))
		}
	case .Pick_Up:
		v.title = "Pick Up What?"
		append(&choices, NEVER_MIND)
		for i in items_in(g, .Ground, g.characters[a].loc) {
			append(&choices, item_label(g, i))
		}
	case .Inventory:
		v.title = tp("%s's Inventory:", char_name(g, a))
		append(&choices, NEVER_MIND)
		for i in items_in(g, .Carried, a) {
			append(&choices, item_label(g, i))
		}
	case .Item_Detail:
		i := g.pick_item
		v.title = item_label(g, i)
		append(&choices, NEVER_MIND)
		if item_descs[g.items[i].type].slot != nil {
			append(&choices, "Equip")
		}
		if can_use(g, a, i) {
			append(&choices, item_descs[g.items[i].type].use_verb)
		}
	case .Equipment:
		v.title = tp("%s's Equipment:", char_name(g, a))
		append(&choices, NEVER_MIND)
		for s in Equip_Slot {
			i := g.characters[a].equipment[s]
			if i >= 0 {
				append(&choices, tp("%s: %s", equip_slot_name(s), item_name(g.items[i].type)))
			}
		}
	case .Equip_Slot:
		i := g.characters[a].equipment[g.pick_slot]
		if i >= 0 {
			t := g.items[i].type
			d := item_descs[t]
			append(&lines, tp("Equip Slot: %s", equip_slot_name(g.pick_slot)))
			append(&lines, tp("Item: %s", d.name))
			if is_weapon_type(t) {
				append(&lines, tp("Attack: +%d/+%d", d.attack, d.max_attack))
			}
			if is_armor_type(t) {
				append(&lines, tp("Defend: +%d/+%d", d.defend, d.max_defend))
			}
			append(&lines, tp("Durability: %d/%d", durability(g, i), d.durability))
		}
		v.title = "Now What?"
		append(&choices, NEVER_MIND, "Unequip")
	case .Interact:
		v.title = "Interact With..."
		append(&choices, NEVER_MIND)
		loc := g.locations[g.characters[a].loc]
		for k in 0 ..< loc.feature_count {
			append(&choices, feature_name(loc.features[k].kind))
		}
	case .Shoppe:
		f := &g.locations[g.characters[a].loc].features[g.pick_feature]
		v.title = feature_title(f.kind)
		append(&choices, NEVER_MIND)
		table := shop_trades(f.kind)
		for t in open_trades(f) {
			trade := table[t]
			append(
				&choices,
				tp(
					"%s(x%d) -> %s(x%d)",
					item_name(trade.from),
					trade.from_qty,
					item_name(trade.to),
					trade.to_qty,
				),
			)
		}
	case .Trainer:
		f := &g.locations[g.characters[a].loc].features[g.pick_feature]
		v.title = feature_title(f.kind)
		append(&choices, NEVER_MIND)
		append(&choices, tp("Maximum Health (%d XP)", train_cost(g, a)))
	case .Status:
		append(&lines, tp("%s's Status:", char_name(g, a)))
		append(&lines, tp("Attack : %d", max_attack(g, a)))
		append(&lines, tp("Defend : %d", max_defend(g, a)))
		append(&lines, tp("Health : %d/%d", health(g, a), g.characters[a].max_health))
		append(&lines, tp("XP : %d", g.characters[a].xp))
		append(&choices, "Ok")
	}
	v.lines = lines[:]
	v.choices = choices[:]
	return v
}

// ---- what the player does

// A choice by its position in the list make_view gave. Anything out of range does nothing.
choose :: proc(g: ^Game, index: int) {
	if index < 0 {
		return
	}
	a := g.avatar
	switch g.screen {
	case .Title:
		g.screen = .Main_Menu
	case .Main_Menu:
		if g.has_world {
			switch index {
			case 0:
				after(g, .Navigation)
			case 1:
				save_game(g)
			case 2:
				g.screen = .Confirm_Abandon
			}
		} else {
			switch index {
			case 0:
				g.screen = .Name_Prompt
			case 1:
				load_game(g)
			}
		}
	case .Name_Prompt:
		// a typed answer, see submit_text
	case .Confirm_Abandon:
		if index == 1 {
			g.has_world = false
		}
		g.screen = .Main_Menu
	case .Navigation:
		actions := nav_actions(g)
		if index >= len(actions) {
			return
		}
		switch actions[index] {
		case .Move:
			g.screen = .Move
		case .Fight:
			fight(g)
			after(g, .Navigation)
		case .Run:
			name := char_name(g, a)
			ran := run_away(g, a)
			push_message_first(g, tp("%s runs away!", name) if ran else tp("%s fails to run away!", name))
			if !ran {
				counter_attacks(g)
			}
			after(g, .Navigation)
		case .Pick_Up:
			g.screen = .Pick_Up
		case .Inventory:
			g.screen = .Inventory
		case .Equipment:
			g.screen = .Equipment
		case .Interact:
			g.screen = .Interact
		case .Status:
			g.screen = .Status
		case .Game_Menu:
			g.screen = .Main_Menu
		}
	case .Message:
		pop_message(g)
		if g.message_count == 0 {
			arrive(g, g.ret)
		}
	case .Game_Over:
		g.has_world = false
		g.screen = .Main_Menu
	case .Move:
		loc := g.locations[g.characters[a].loc]
		if index == 0 || index > loc.route_count {
			g.screen = .Navigation
			return
		}
		move(g, a, loc.order[index - 1])
		after(g, .Navigation)
	case .Pick_Up:
		items := items_in(g, .Ground, g.characters[a].loc)
		if index == 0 || index > len(items) {
			g.screen = .Navigation
			return
		}
		i := items[index - 1]
		push_message(g, tp("%s takes %s.", char_name(g, a), item_label(g, i)))
		put_item(g, i, .Carried, a)
		after(g, .Navigation)
	case .Inventory:
		items := items_in(g, .Carried, a)
		if index == 0 || index > len(items) {
			g.screen = .Navigation
			return
		}
		g.pick_item = items[index - 1]
		g.screen = .Item_Detail
	case .Item_Detail:
		i := g.pick_item
		if index == 0 || g.items[i].type == .None || g.items[i].place != .Carried || g.items[i].owner != a {
			g.screen = .Navigation
			return
		}
		equippable := item_descs[g.items[i].type].slot != nil
		if equippable && index == 1 {
			equip(g, a, i)
			after(g, .Navigation)
		} else if can_use(g, a, i) && index == (2 if equippable else 1) {
			use_item(g, a, i)
			after(g, .Navigation)
		} else {
			g.screen = .Navigation
		}
	case .Equipment:
		if index == 0 {
			g.screen = .Navigation
			return
		}
		seen := 0
		for s in Equip_Slot {
			if g.characters[a].equipment[s] >= 0 {
				seen += 1
				if seen == index {
					g.pick_slot = s
					g.screen = .Equip_Slot
					return
				}
			}
		}
		g.screen = .Navigation
	case .Equip_Slot:
		if index == 1 {
			unequip(g, a, g.pick_slot)
			after(g, .Equipment)
		} else {
			g.screen = .Equipment
		}
	case .Interact:
		loc := g.locations[g.characters[a].loc]
		if index == 0 || index > loc.feature_count {
			g.screen = .Navigation
			return
		}
		slot := index - 1
		g.pick_feature = slot
		switch feature_class(loc.features[slot].kind) {
		case .Shoppe:
			g.screen = .Shoppe
		case .Trainer:
			g.screen = .Trainer
		case .Loot:
			loot(g, a, slot)
			after(g, .Navigation)
		case .Spawner:
			spawn_from(g, a, slot)
			after(g, .Navigation)
		}
	case .Shoppe:
		f := &g.locations[g.characters[a].loc].features[g.pick_feature]
		open := open_trades(f)
		if index == 0 || index > len(open) {
			g.screen = .Navigation
			return
		}
		do_trade(g, a, f, open[index - 1])
		after(g, .Shoppe)
	case .Trainer:
		if index == 1 {
			train(g, a)
			after(g, .Trainer)
		} else {
			g.screen = .Navigation
		}
	case .Status:
		g.screen = .Navigation
	}
}

// The answer to a typed question.
submit_text :: proc(g: ^Game, text: string) {
	if g.screen != .Name_Prompt {
		return
	}
	name := strings.trim_space(text)
	if len(name) == 0 {
		name = "N00b"
	}
	create_world(g)
	set_char_name(g, g.avatar, name)
	g.screen = .Navigation
}

// ---- saving and loading

save_game :: proc(g: ^Game) {
	ok := storage.write != nil && storage.write(serialize_game(g))
	push_message(g, "Saved game!" if ok else "Saving Game Failed!")
	g.ret = .Main_Menu
	g.screen = .Message
}

load_game :: proc(g: ^Game) {
	ok := load_saved(g)
	push_message(g, "Loaded game!" if ok else "Loading Game Failed!")
	g.ret = .Navigation if ok else .Main_Menu
	g.screen = .Message
}

// Reads the saved game into g. Nothing changes unless the whole save is good.
load_saved :: proc(g: ^Game) -> bool {
	if storage.read == nil {
		return false
	}
	text, ok := storage.read()
	if !ok {
		return false
	}
	return parse_game(g, text)
}

// Called after every answer: a game in progress is saved when the player is back at a quiet point, and a game that
// is over leaves no save behind.
settle :: proc(g: ^Game) {
	if storage.write == nil {
		return
	}
	if !g.has_world {
		if storage.clear != nil {
			storage.clear()
		}
	} else if g.message_count == 0 && g.screen != .Game_Over && g.screen != .Title {
		storage.write(serialize_game(g))
	}
}
