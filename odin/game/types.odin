// Key Seeker of SPLORR!!: the rules, as plain data. The original was a VB.NET console game (ASCII Game Jam 2023).
// Nothing in this package reads a clock or the page; the random seed is an argument, so tests can force any world.
package game

NAME_MAX :: 24
MAX_LOCATIONS :: 512
MAX_CHARACTERS :: 1024
MAX_ITEMS :: 8192
MAX_LOC_FEATURES :: 4
MAX_TRADES :: 10
MAX_MESSAGES :: 32
MESSAGE_BYTES :: 1024
UNLIMITED :: 1 << 30 // a trade that never runs out

Direction :: enum u8 {
	North,
	East,
	South,
	West,
	Inside,
	Outside,
	Up,
	Down,
	Portal,
}
DIRECTION_COUNT :: len(Direction)

Route_Type :: enum u8 {
	Forest_Path,
	Dirt_Road,
	Town_Gate,
	Graveyard_Path,
	Graveyard_Gate,
	Ruins_Entrance,
	Ruins_Path,
	Door,
	Stairs,
	Tunnel,
	Tower_Entrance,
	Portal,
	Hallway,
	Ufo_Ramp,
	Hatch,
}

Location_Type :: enum u8 {
	Forest_Corner,
	Forest,
	Forest_Center,
	Town,
	Town_Edge,
	Graveyard,
	Ruins,
	Inn,
	Cellar,
	Sewer,
	Knackery,
	Blacksmith,
	Tower,
	Hut,
	Ufo,
	Machine,
}

Item_Type :: enum u8 {
	None, // a free slot in the item table
	Sewer_Key,
	Graveyard_Key,
	Tower_Key,
	Ufo_Key,
	Machine_Key,
	Jools,
	Chikkin,
	Cellar_Rat_Tail,
	Sewer_Rat_Tail,
	Dagger,
	Shortsword,
	Broadsword,
	Longsword,
	Axe,
	Shield,
	Helmet,
	Leather_Armor,
	Chain_Mail,
	Plate_Mail,
	Skull_Fragment,
	Zombie_Taint,
	Feather,
	Goblin_Ear,
	Orc_Tooth,
	Cyclops_Eyeball,
	Potion,
	Holy_Water,
	Town_Portal,
	Empty_Bottle,
	Marble_Chunk,
}

Equip_Slot :: enum u8 {
	Weapon,
	Shield,
	Torso,
	Head,
}

// The order is the order the original spawned them in at the start of a game.
Character_Type :: enum u8 {
	N00b,
	Skeleton,
	Zombie,
	Blob,
	Cellar_Rat,
	Sewer_Rat,
	King_Rat,
	Goblin,
	Orc,
	Cyclops,
	Moon_Person,
	Mummy,
	Gargoyle,
	Guardian,
}

Feature_Kind :: enum u8 {
	None,
	Innkeeper,
	Knacker,
	Blacksmith,
	Healer,
	Elder,
	Yogi,
	Grave_Spawner,
	Grave_Loot,
	Wooden_Chest,
	Metal_Box,
	Lever,
}

Feature_Class :: enum u8 {
	Shoppe,
	Trainer,
	Loot,
	Spawner,
}

Route :: struct {
	present:    bool,
	to:         int,
	type:       Route_Type,
	required:   Item_Type, // .None means the way is open
	single_use: bool,
}

// A shop, trainer, grave, chest or lever standing in a location.
Feature :: struct {
	kind:     Feature_Kind,
	loot_type: Item_Type,
	loot_qty: int,
	avail:    [MAX_TRADES]int, // how many times each trade can still be done; UNLIMITED for most
}

Location :: struct {
	type:          Location_Type,
	routes:        [Direction]Route,
	order:         [DIRECTION_COUNT]Direction, // routes in the order they were made; the menus list them this way
	route_count:   int,
	features:      [MAX_LOC_FEATURES]Feature,
	feature_count: int,
}

Place :: enum u8 {
	Limbo,
	Carried,
	Equipped,
	Ground,
}

Item :: struct {
	type:  Item_Type, // .None marks a free slot
	qty:   int,
	wear:  int,
	order: int, // when it was put where it is; lists show items oldest first
	place: Place,
	owner: int, // a character for Carried and Equipped, a location for Ground
}

Character :: struct {
	alive:      bool,
	type:       Character_Type,
	loc:        int,
	name:       [NAME_MAX]u8,
	name_len:   int,
	max_health: int,
	wounds:     int,
	xp:         int,
	equipment:  [Equip_Slot]int, // item index, or -1
}

Message :: struct {
	text: [MESSAGE_BYTES]u8, // lines joined by '\n'
	len:  int,
}

Screen :: enum u8 {
	Title,
	Main_Menu,
	Name_Prompt,
	Confirm_Abandon,
	Navigation,
	Message,
	Game_Over,
	Move,
	Pick_Up,
	Inventory,
	Item_Detail,
	Equipment,
	Equip_Slot,
	Interact,
	Shoppe,
	Trainer,
	Status,
}

Rng :: struct {
	state: u64,
}

Game :: struct {
	rng:         Rng,
	has_world:   bool,
	avatar:      int,
	locations:   [MAX_LOCATIONS]Location,
	location_count: int,
	characters:  [MAX_CHARACTERS]Character,
	character_count: int,
	items:       [MAX_ITEMS]Item,
	next_order:  int,
	messages:    [MAX_MESSAGES]Message,
	message_count: int,

	// Where the player is in the menus. None of this is saved.
	screen:      Screen,
	ret:         Screen, // where a dismissed message goes next
	pick_item:   int, // the inventory item being looked at
	pick_slot:   Equip_Slot,
	pick_feature: int, // the feature being used, as an index into the location's features
}
