package game

// ---- names (the original's tables, word for word)

location_type_name :: proc(t: Location_Type) -> string {
	switch t {
	case .Forest_Corner, .Forest, .Forest_Center:
		return "a forest"
	case .Town, .Town_Edge:
		return "town"
	case .Graveyard:
		return "a graveyard"
	case .Ruins:
		return "a ruins"
	case .Inn:
		return "an inn"
	case .Cellar:
		return "a cellar"
	case .Sewer:
		return "a sewer"
	case .Knackery:
		return "a knackery"
	case .Blacksmith:
		return "a forge"
	case .Tower:
		return "a tower"
	case .Hut:
		return "a hut"
	case .Ufo:
		return "a UFO"
	case .Machine:
		return "a machine"
	}
	return ""
}

direction_name :: proc(d: Direction) -> string {
	switch d {
	case .North:
		return "North"
	case .East:
		return "East"
	case .South:
		return "South"
	case .West:
		return "West"
	case .Inside:
		return "In"
	case .Outside:
		return "Out"
	case .Up:
		return "Up"
	case .Down:
		return "Down"
	case .Portal:
		return "into a Portal"
	}
	return ""
}

route_type_name :: proc(t: Route_Type) -> string {
	switch t {
	case .Forest_Path:
		return "Forest Path"
	case .Dirt_Road:
		return "Dirt Road"
	case .Town_Gate:
		return "Town Gate"
	case .Graveyard_Path:
		return "Graveyard Path"
	case .Graveyard_Gate:
		return "Graveyard Gate"
	case .Ruins_Entrance:
		return "Ruins Entrance"
	case .Ruins_Path:
		return "Ruins Path"
	case .Door:
		return "Door"
	case .Stairs:
		return "Stairs"
	case .Tunnel:
		return "Tunnel"
	case .Tower_Entrance:
		return "Tower Entrance"
	case .Portal:
		return "Shimmering Oval"
	case .Hallway:
		return "Metallic Hallway"
	case .Ufo_Ramp:
		return "Otherworldly Ramp"
	case .Hatch:
		return "Circular Hatch"
	}
	return ""
}

equip_slot_name :: proc(s: Equip_Slot) -> string {
	switch s {
	case .Weapon:
		return "Weapon"
	case .Shield:
		return "Shield"
	case .Torso:
		return "Torso"
	case .Head:
		return "Head"
	}
	return ""
}

// ---- items

Item_Desc :: struct {
	name:       string,
	stacks:     bool,
	slot:       Maybe(Equip_Slot), // set when it can be equipped
	attack:     int, // dice added to the wearer's attack (weapons)
	max_attack: int,
	defend:     int, // dice added to the wearer's defend (armour)
	max_defend: int,
	durability: int,
	use_verb:   string, // empty when it cannot be used
}

item_descs := [Item_Type]Item_Desc {
	.None = {name = ""},
	.Sewer_Key = {name = "Sewer Key"},
	.Graveyard_Key = {name = "Graveyard Key"},
	.Tower_Key = {name = "Tower Key"},
	.Ufo_Key = {name = "UFO Key"},
	.Machine_Key = {name = "Machine Key"},
	.Jools = {name = "Jools", stacks = true},
	.Chikkin = {name = "Chikkin", stacks = true, use_verb = "Nom"},
	.Cellar_Rat_Tail = {name = "Cellar Rat Tail", stacks = true},
	.Sewer_Rat_Tail = {name = "Sewer Rat Tail", stacks = true},
	.Dagger = {name = "Dagger", slot = .Weapon, attack = 1, max_attack = 0, durability = 20},
	.Shortsword = {name = "Shortsword", slot = .Weapon, attack = 4, max_attack = 1, durability = 40},
	.Broadsword = {name = "Broadsword", slot = .Weapon, attack = 7, max_attack = 2, durability = 60},
	.Longsword = {name = "Longsword", slot = .Weapon, attack = 10, max_attack = 3, durability = 80},
	.Axe = {name = "Axe", slot = .Weapon, attack = 13, max_attack = 4, durability = 100},
	.Shield = {name = "Shield", slot = .Shield, defend = 2, max_defend = 1, durability = 50},
	.Helmet = {name = "Helmet", slot = .Head, defend = 1, max_defend = 0, durability = 25},
	.Leather_Armor = {name = "Leather Armor", slot = .Torso, defend = 2, max_defend = 1, durability = 25},
	.Chain_Mail = {name = "Chain Mail", slot = .Torso, defend = 2, max_defend = 1, durability = 100},
	.Plate_Mail = {name = "Plate Mail", slot = .Torso, defend = 4, max_defend = 2, durability = 150},
	.Skull_Fragment = {name = "Skull Fragment", stacks = true},
	.Zombie_Taint = {name = "Zombie Taint", stacks = true},
	.Feather = {name = "Feather", stacks = true},
	.Goblin_Ear = {name = "Goblin Ear", stacks = true},
	.Orc_Tooth = {name = "Orc Tooth", stacks = true},
	.Cyclops_Eyeball = {name = "Cyclops Eyeball", stacks = true},
	.Potion = {name = "Potion", stacks = true, use_verb = "Drink"},
	.Holy_Water = {name = "Holy Water", stacks = true, use_verb = "Splash"},
	.Town_Portal = {name = "Scroll of Town Portal", use_verb = "Read"},
	.Empty_Bottle = {name = "Empty Bottle", stacks = true},
	.Marble_Chunk = {name = "Chunk of Marble", stacks = true},
}

is_weapon_type :: proc(t: Item_Type) -> bool {
	slot, ok := item_descs[t].slot.?
	return ok && slot == .Weapon
}

is_armor_type :: proc(t: Item_Type) -> bool {
	slot, ok := item_descs[t].slot.?
	return ok && slot != .Weapon
}

// ---- characters

Char_Desc :: struct {
	name:        string,
	spawn_count: int,
	spawn_at:    Location_Type,
	kill_verb:   string,
	max_health:  int,
	max_attack:  int,
	attack:      int, // dice
	max_defend:  int,
	defend:      int, // dice
	xp:          int,
}

char_descs := [Character_Type]Char_Desc {
	.N00b = {"N00b", 1, .Town, "kills", 3, 1, 3, 2, 4, 0},
	.Skeleton = {"Skeleton", 50, .Graveyard, "kills", 1, 2, 4, 2, 2, 1},
	.Zombie = {"Zombie", 25, .Graveyard, "consumes the brains of", 1, 2, 4, 3, 3, 1},
	.Blob = {"Blob", 300, .Forest, "slimes", 1, 1, 2, 1, 1, 1},
	.Cellar_Rat = {"Cellar Rat", 5, .Cellar, "gnaws the flesh of", 1, 1, 2, 1, 1, 1},
	.Sewer_Rat = {"Sewer Rat", 75, .Sewer, "feasts on the innards of", 1, 1, 4, 1, 1, 1},
	.King_Rat = {"King Rat", 1, .Sewer, "cavorts in the intestines of", 2, 2, 4, 2, 2, 2},
	.Goblin = {"Goblin", 75, .Tower, "dances on the corpse of", 1, 2, 4, 1, 1, 1},
	.Orc = {"Orc", 50, .Tower, "harvests the heart of", 1, 3, 6, 2, 2, 1},
	.Cyclops = {"Cyclops", 25, .Tower, "plucks out one eye of", 2, 3, 6, 3, 3, 2},
	.Moon_Person = {"Moon Person", 20, .Ufo, "does lewd experiments on", 3, 4, 8, 4, 4, 3},
	.Mummy = {"Mummy", 30, .Ruins, "mummifies", 2, 3, 6, 4, 4, 2},
	.Gargoyle = {"Gargoyle", 15, .Ruins, "paints itself in the blood of", 3, 4, 8, 5, 5, 3},
	.Guardian = {"Machine Guardian", 15, .Ruins, "makes a greasy spot where once stood", 10, 10, 20, 10, 10, 10},
}

// ---- features

feature_class :: proc(k: Feature_Kind) -> Feature_Class {
	switch k {
	case .Innkeeper, .Knacker, .Blacksmith, .Healer, .Elder:
		return .Shoppe
	case .Yogi:
		return .Trainer
	case .Grave_Loot, .Wooden_Chest, .Metal_Box:
		return .Loot
	case .Grave_Spawner, .Lever:
		return .Spawner
	case .None:
	}
	return .Loot
}

feature_name :: proc(k: Feature_Kind) -> string {
	switch k {
	case .None:
		return ""
	case .Innkeeper:
		return "Gurachan The Innkeeper"
	case .Knacker:
		return "\"Honest\" Dan the Knacker"
	case .Blacksmith:
		return "Samuli's Blacksmithery"
	case .Healer:
		return "Mårten the Nihilist Healer"
	case .Elder:
		return "Zooperdan the Elder"
	case .Yogi:
		return "Yogi the Bare Bear Yoga Instructor"
	case .Grave_Spawner, .Grave_Loot:
		return "grave"
	case .Wooden_Chest:
		return "Wooden Chest"
	case .Metal_Box:
		return "Metal Box"
	case .Lever:
		return "lever"
	}
	return ""
}

// What a shop or trainer calls itself once you are talking to it.
feature_title :: proc(k: Feature_Kind) -> string {
	switch k {
	case .Innkeeper:
		return "Resting Dog's Face Inn"
	case .Knacker:
		return "\"Honest\" Dan's Knackery"
	case .Blacksmith:
		return "Samuli's Wares"
	case .Healer:
		return "Mårten's Apathetic Apothecary"
	case .Elder:
		return "Zooperdan's Bargain Town Portal Scrolls"
	case .Yogi:
		return "Yogi The Bare Bear's Personal Training"
	case .None, .Grave_Spawner, .Grave_Loot, .Wooden_Chest, .Metal_Box, .Lever:
	}
	return ""
}

Trade :: struct {
	from:     Item_Type,
	from_qty: int,
	to:       Item_Type,
	to_qty:   int,
	limit:    int, // 0 means as often as you like
}

inn_trades := [?]Trade{{.Jools, 2, .Chikkin, 1, 0}, {.Cellar_Rat_Tail, 5, .Sewer_Key, 1, 1}}
knacker_trades := [?]Trade {
	{.Sewer_Rat_Tail, 3, .Jools, 1, 0},
	{.Sewer_Rat_Tail, 7, .Jools, 2, 0},
	{.Skull_Fragment, 4, .Jools, 1, 0},
	{.Zombie_Taint, 2, .Jools, 1, 0},
	{.Feather, 1, .Jools, 1, 0},
	{.Goblin_Ear, 1, .Jools, 2, 0},
	{.Orc_Tooth, 1, .Jools, 3, 0},
	{.Cyclops_Eyeball, 1, .Jools, 5, 0},
	{.Empty_Bottle, 2, .Jools, 5, 0},
	{.Marble_Chunk, 1, .Jools, 10, 0},
}
blacksmith_trades := [?]Trade {
	{.Jools, 25, .Dagger, 1, 0},
	{.Jools, 50, .Shortsword, 1, 0},
	{.Jools, 75, .Broadsword, 1, 0},
	{.Jools, 100, .Longsword, 1, 0},
	{.Jools, 125, .Axe, 1, 0},
	{.Jools, 50, .Shield, 1, 0},
	{.Jools, 25, .Helmet, 1, 0},
	{.Jools, 50, .Leather_Armor, 1, 0},
	{.Jools, 100, .Chain_Mail, 1, 0},
	{.Jools, 150, .Plate_Mail, 1, 0},
}
healer_trades := [?]Trade{{.Jools, 9, .Potion, 1, 0}, {.Jools, 3, .Holy_Water, 1, 0}}
elder_trades := [?]Trade{{.Jools, 25, .Town_Portal, 1, 0}}

shop_trades :: proc(k: Feature_Kind) -> []Trade {
	switch k {
	case .Innkeeper:
		return inn_trades[:]
	case .Knacker:
		return knacker_trades[:]
	case .Blacksmith:
		return blacksmith_trades[:]
	case .Healer:
		return healer_trades[:]
	case .Elder:
		return elder_trades[:]
	case .None, .Yogi, .Grave_Spawner, .Grave_Loot, .Wooden_Chest, .Metal_Box, .Lever:
	}
	return nil
}

// Training: what a statistic costs is its multiplier times what it is now.
YOGI_HEALTH_MULTIPLIER :: 10
