package game

// ---- building blocks

create_location :: proc(g: ^Game, type: Location_Type) -> int {
	i := g.location_count
	g.locations[i] = Location {
		type = type,
	}
	g.location_count += 1
	return i
}

// Makes (or replaces) the exit in a direction. A replaced exit keeps its place in the list.
add_route :: proc(g: ^Game, from: int, d: Direction, type: Route_Type, to: int) -> ^Route {
	loc := &g.locations[from]
	if !loc.routes[d].present {
		loc.order[loc.route_count] = d
		loc.route_count += 1
	}
	loc.routes[d] = Route {
		present = true,
		to      = to,
		type    = type,
	}
	return &loc.routes[d]
}

remove_route :: proc(g: ^Game, from: int, d: Direction) {
	loc := &g.locations[from]
	if !loc.routes[d].present {
		return
	}
	loc.routes[d] = {}
	at := 0
	for k in 0 ..< loc.route_count {
		if loc.order[k] == d {
			at = k
		}
	}
	for k in at ..< loc.route_count - 1 {
		loc.order[k] = loc.order[k + 1]
	}
	loc.route_count -= 1
}

add_feature :: proc(g: ^Game, at: int, f: Feature) {
	loc := &g.locations[at]
	loc.features[loc.feature_count] = f
	loc.feature_count += 1
}

remove_feature :: proc(g: ^Game, at: int, slot: int) {
	loc := &g.locations[at]
	for k in slot ..< loc.feature_count - 1 {
		loc.features[k] = loc.features[k + 1]
	}
	loc.feature_count -= 1
	loc.features[loc.feature_count] = {}
}

make_feature :: proc(kind: Feature_Kind, loot_type: Item_Type = .None, loot_qty: int = 0) -> Feature {
	f := Feature {
		kind      = kind,
		loot_type = loot_type,
		loot_qty  = loot_qty,
	}
	for k in 0 ..< MAX_TRADES {
		f.avail[k] = UNLIMITED
	}
	for t, k in shop_trades(kind) {
		if t.limit > 0 {
			f.avail[k] = t.limit
		}
	}
	return f
}

// A random location of a type, optionally one that has no exit yet in a direction. -1 if there is none.
pick_location :: proc(g: ^Game, type: Location_Type, free_dir: Maybe(Direction) = nil) -> int {
	found: [MAX_LOCATIONS]int
	n := 0
	for i in 0 ..< g.location_count {
		if g.locations[i].type != type {
			continue
		}
		if d, ok := free_dir.?; ok && g.locations[i].routes[d].present {
			continue
		}
		found[n] = i
		n += 1
	}
	if n == 0 {
		return -1
	}
	return found[below(&g.rng, n)]
}

// ---- mazes (the original's randomised Prim: grow from one cell by opening a door from a random edge cell)

MAZE_CELLS_MAX :: 121

Maze :: struct {
	cols, rows: int,
	open:       [MAZE_CELLS_MAX][4]bool, // a door per cell, by North, East, South, West
}

// The order the original listed a cell's neighbours in: the ones made before it (west, north), then east, south.
MAZE_ORDER :: [4]Direction{.West, .North, .East, .South}

maze_step :: proc(d: Direction) -> (dx, dy: int) {
	#partial switch d {
	case .North:
		return 0, -1
	case .East:
		return 1, 0
	case .South:
		return 0, 1
	case .West:
		return -1, 0
	}
	return 0, 0
}

opposite :: proc(d: Direction) -> Direction {
	#partial switch d {
	case .North:
		return .South
	case .East:
		return .West
	case .South:
		return .North
	case .West:
		return .East
	}
	return d
}

// The cell next door, or -1 at the edge.
maze_neighbor :: proc(m: ^Maze, cell: int, d: Direction) -> int {
	dx, dy := maze_step(d)
	col := cell % m.cols + dx
	row := cell / m.cols + dy
	if col < 0 || row < 0 || col >= m.cols || row >= m.rows {
		return -1
	}
	return col + row * m.cols
}

maze_generate :: proc(g: ^Game, cols, rows: int) -> Maze {
	m := Maze {
		cols = cols,
		rows = rows,
	}
	cells := cols * rows
	inside: [MAZE_CELLS_MAX]bool
	in_frontier: [MAZE_CELLS_MAX]bool
	frontier: [MAZE_CELLS_MAX]int
	count := 0

	start_col := range_of(&g.rng, 0, cols - 1)
	start_row := range_of(&g.rng, 0, rows - 1)
	start := start_col + start_row * cols
	inside[start] = true
	for d in MAZE_ORDER {
		n := maze_neighbor(&m, start, d)
		if n >= 0 && !in_frontier[n] {
			frontier[count] = n
			count += 1
			in_frontier[n] = true
		}
	}
	for count > 0 {
		at := below(&g.rng, count)
		cell := frontier[at]
		toward: [4]Direction
		k := 0
		for d in MAZE_ORDER {
			n := maze_neighbor(&m, cell, d)
			if n >= 0 && inside[n] {
				toward[k] = d
				k += 1
			}
		}
		d := toward[below(&g.rng, k)]
		n := maze_neighbor(&m, cell, d)
		m.open[cell][int(d)] = true
		m.open[n][int(opposite(d))] = true
		inside[cell] = true
		in_frontier[cell] = false
		frontier[at] = frontier[count - 1]
		count -= 1
		for e in MAZE_ORDER {
			nn := maze_neighbor(&m, cell, e)
			if nn >= 0 && !inside[nn] && !in_frontier[nn] {
				frontier[count] = nn
				count += 1
				in_frontier[nn] = true
			}
		}
	}
	_ = cells
	return m
}

// Makes the locations of a grid (column by column, like the original) and returns the first one's index.
// The grid's cell (col, row) is location first + col * rows + row.
make_grid :: proc(g: ^Game, cols, rows: int, type: Location_Type, type_of: proc(col, row, cols, rows: int) -> Location_Type = nil) -> int {
	first := g.location_count
	for col in 0 ..< cols {
		for row in 0 ..< rows {
			create_location(g, type if type_of == nil else type_of(col, row, cols, rows))
		}
	}
	return first
}

grid_at :: proc(first, rows, col, row: int) -> int {
	return first + col * rows + row
}

// Opens the maze's doors as routes of one type.
connect_maze :: proc(g: ^Game, m: ^Maze, first: int, type: Route_Type) {
	for col in 0 ..< m.cols {
		for row in 0 ..< m.rows {
			cell := col + row * m.cols
			for d in MAZE_ORDER {
				if m.open[cell][int(d)] {
					dx, dy := maze_step(d)
					add_route(g, grid_at(first, m.rows, col, row), d, type, grid_at(first, m.rows, col + dx, row + dy))
				}
			}
		}
	}
}

// ---- the places

TOWN_COLS :: 3
TOWN_ROWS :: 3
FOREST_COLS :: 11
FOREST_ROWS :: 11
GRAVEYARD_COLS :: 7
GRAVEYARD_ROWS :: 7
RUINS_COLS :: 6
RUINS_ROWS :: 6
SEWER_COLS :: 6
SEWER_ROWS :: 6
TOWER_COLS :: 3
TOWER_ROWS :: 3
TOWER_LEVELS :: 7
UFO_COLS :: 5
UFO_ROWS :: 5

forest_type :: proc(col, row, cols, rows: int) -> Location_Type {
	if col == cols / 2 && row == rows / 2 {
		return .Forest_Center
	}
	if (col == 0 || col == cols - 1) && (row == 0 || row == rows - 1) {
		return .Forest_Corner
	}
	return .Forest
}

town_type :: proc(col, row, cols, rows: int) -> Location_Type {
	if col == 0 || col == cols - 1 || row == 0 || row == rows - 1 {
		return .Town_Edge
	}
	return .Town
}

make_forest :: proc(g: ^Game) {
	first := make_grid(g, FOREST_COLS, FOREST_ROWS, .Forest, forest_type)
	m := maze_generate(g, FOREST_COLS, FOREST_ROWS)
	connect_maze(g, &m, first, .Forest_Path)
}

// A building: a door in from a town edge, a door back out.
make_building :: proc(g: ^Game, type: Location_Type) -> int {
	entrance := pick_location(g, .Town_Edge, Direction.Inside)
	building := create_location(g, type)
	add_route(g, entrance, .Inside, .Door, building)
	add_route(g, building, .Outside, .Door, entrance)
	return building
}

make_town :: proc(g: ^Game) {
	first := make_grid(g, TOWN_COLS, TOWN_ROWS, .Town, town_type)
	m := maze_generate(g, TOWN_COLS, TOWN_ROWS)
	connect_maze(g, &m, first, .Dirt_Road)
	forest_center := pick_location(g, .Forest_Center)
	add_route(g, grid_at(first, TOWN_ROWS, TOWN_COLS / 2, 0), .North, .Town_Gate, forest_center)
	add_route(g, grid_at(first, TOWN_ROWS, TOWN_COLS / 2, TOWN_ROWS - 1), .South, .Town_Gate, forest_center)
	add_route(g, grid_at(first, TOWN_ROWS, 0, TOWN_ROWS / 2), .West, .Town_Gate, forest_center)
	add_route(g, grid_at(first, TOWN_ROWS, TOWN_COLS - 1, TOWN_ROWS / 2), .East, .Town_Gate, forest_center)
	add_route(g, forest_center, .Inside, .Town_Gate, grid_at(first, TOWN_ROWS, TOWN_COLS / 2, TOWN_ROWS - 1))

	inn := make_building(g, .Inn)
	cellar := create_location(g, .Cellar)
	add_route(g, inn, .Down, .Stairs, cellar)
	add_route(g, cellar, .Up, .Stairs, inn)
	add_feature(g, inn, make_feature(.Innkeeper))

	knackery := make_building(g, .Knackery)
	add_feature(g, knackery, make_feature(.Knacker))

	add_feature(g, pick_location(g, .Town_Edge), make_feature(.Yogi))

	forge := make_building(g, .Blacksmith)
	add_feature(g, forge, make_feature(.Blacksmith))

	hut := make_building(g, .Hut)
	add_feature(g, hut, make_feature(.Healer))

	add_feature(g, pick_location(g, .Town_Edge), make_feature(.Elder))
}

make_graveyard :: proc(g: ^Game) {
	first := make_grid(g, GRAVEYARD_COLS, GRAVEYARD_ROWS, .Graveyard)
	for col in 0 ..< GRAVEYARD_COLS {
		for row in 0 ..< GRAVEYARD_ROWS {
			here := grid_at(first, GRAVEYARD_ROWS, col, row)
			if row > 0 {
				add_route(g, here, .North, .Graveyard_Path, grid_at(first, GRAVEYARD_ROWS, col, row - 1))
			}
			if col > 0 {
				add_route(g, here, .West, .Graveyard_Path, grid_at(first, GRAVEYARD_ROWS, col - 1, row))
			}
			if row < GRAVEYARD_ROWS - 1 {
				add_route(g, here, .South, .Graveyard_Path, grid_at(first, GRAVEYARD_ROWS, col, row + 1))
			}
			if col < GRAVEYARD_COLS - 1 {
				add_route(g, here, .East, .Graveyard_Path, grid_at(first, GRAVEYARD_ROWS, col + 1, row))
			}
		}
	}
	gate := grid_at(first, GRAVEYARD_ROWS, GRAVEYARD_COLS / 2, GRAVEYARD_ROWS - 1)
	forest := pick_location(g, .Forest_Corner, Direction.Inside)
	add_route(g, forest, .Inside, .Graveyard_Gate, gate).required = .Graveyard_Key
	add_route(g, gate, .South, .Graveyard_Gate, forest)

	// every grave holds something; one of them holds the key to the tower
	key_grave := first + below(&g.rng, GRAVEYARD_COLS * GRAVEYARD_ROWS)
	for l in first ..< first + GRAVEYARD_COLS * GRAVEYARD_ROWS {
		if l == key_grave {
			add_feature(g, l, make_feature(.Grave_Loot, .Tower_Key, 1))
		} else if below(&g.rng, 2) == 0 {
			add_feature(g, l, make_feature(.Grave_Spawner))
		} else {
			add_feature(g, l, make_feature(.Grave_Loot, .Jools, roll_xdy(&g.rng, 2, 6)))
		}
	}
}

make_ruins :: proc(g: ^Game) {
	first := make_grid(g, RUINS_COLS, RUINS_ROWS, .Ruins)
	m := maze_generate(g, RUINS_COLS, RUINS_ROWS)
	connect_maze(g, &m, first, .Ruins_Path)
	forest := pick_location(g, .Forest_Corner, Direction.Inside)
	add_route(g, forest, .Inside, .Ruins_Entrance, first)
	add_route(g, first, .Outside, .Ruins_Entrance, forest)

	// the machine is under a random ruin, behind a hatch
	ruin := pick_location(g, .Ruins)
	machine := create_location(g, .Machine)
	add_route(g, ruin, .Down, .Hatch, machine).required = .Machine_Key
	add_route(g, machine, .Up, .Hatch, ruin)
	add_feature(g, machine, make_feature(.Lever))
}

make_sewers :: proc(g: ^Game) {
	first := make_grid(g, SEWER_COLS, SEWER_ROWS, .Sewer)
	m := maze_generate(g, SEWER_COLS, SEWER_ROWS)
	connect_maze(g, &m, first, .Tunnel)
	start_col := range_of(&g.rng, 0, SEWER_COLS - 1)
	start_row := range_of(&g.rng, 0, SEWER_ROWS - 1)
	start := grid_at(first, SEWER_ROWS, start_col, start_row)
	entrance := pick_location(g, .Town_Edge, Direction.Down)
	add_route(g, entrance, .Down, .Stairs, start).required = .Sewer_Key
	add_route(g, start, .Up, .Stairs, entrance)
}

make_tower :: proc(g: ^Game) {
	levels: [TOWER_LEVELS]int
	for level in 0 ..< TOWER_LEVELS {
		levels[level] = make_grid(g, TOWER_COLS, TOWER_ROWS, .Tower)
		m := maze_generate(g, TOWER_COLS, TOWER_ROWS)
		connect_maze(g, &m, levels[level], .Door)
		if level > 0 {
			col := range_of(&g.rng, 0, TOWER_COLS - 1)
			row := range_of(&g.rng, 0, TOWER_ROWS - 1)
			add_route(g, grid_at(levels[level], TOWER_ROWS, col, row), .Down, .Stairs, grid_at(levels[level - 1], TOWER_ROWS, col, row))
			add_route(g, grid_at(levels[level - 1], TOWER_ROWS, col, row), .Up, .Stairs, grid_at(levels[level], TOWER_ROWS, col, row))
		}
		if level == TOWER_LEVELS - 1 {
			col := range_of(&g.rng, 0, TOWER_COLS - 1)
			row := range_of(&g.rng, 0, TOWER_ROWS - 1)
			add_feature(g, grid_at(levels[level], TOWER_ROWS, col, row), make_feature(.Wooden_Chest, .Ufo_Key, 1))
		}
	}
	forest := pick_location(g, .Forest_Corner, Direction.Inside)
	door := grid_at(levels[0], TOWER_ROWS, TOWER_COLS / 2, TOWER_ROWS - 1)
	add_route(g, forest, .Inside, .Tower_Entrance, door).required = .Tower_Key
	add_route(g, door, .Outside, .Tower_Entrance, forest)
}

make_ufo :: proc(g: ^Game) {
	first := make_grid(g, UFO_COLS, UFO_ROWS, .Ufo)
	m := maze_generate(g, UFO_COLS, UFO_ROWS)
	connect_maze(g, &m, first, .Hallway)
	forest := pick_location(g, .Forest_Corner, Direction.Inside)
	center := grid_at(first, UFO_ROWS, UFO_COLS / 2, UFO_ROWS / 2)
	add_route(g, forest, .Up, .Ufo_Ramp, center).required = .Ufo_Key
	add_route(g, center, .Down, .Ufo_Ramp, forest)
	col := range_of(&g.rng, 0, UFO_COLS - 1)
	row := range_of(&g.rng, 0, UFO_ROWS - 1)
	add_feature(g, grid_at(first, UFO_ROWS, col, row), make_feature(.Metal_Box, .Machine_Key, 1))
}

populate :: proc(g: ^Game) {
	for type in Character_Type {
		d := char_descs[type]
		for _ in 0 ..< d.spawn_count {
			at := pick_location(g, d.spawn_at)
			create_character(g, type, at)
		}
	}
}

// ---- a new world

new_game :: proc(seed: u64) -> ^Game {
	g := new(Game)
	g.rng = Rng {
		state = seed,
	}
	g.screen = .Title
	return g
}

// Clears everything but the random numbers and builds the world, with the avatar in the middle of town.
create_world :: proc(g: ^Game) {
	rng := g.rng
	g^ = {}
	g.rng = rng
	g.screen = .Main_Menu
	make_forest(g)
	make_town(g)
	make_graveyard(g)
	make_ruins(g)
	make_sewers(g)
	make_tower(g)
	make_ufo(g)
	populate(g)
	for i in 0 ..< g.character_count {
		if g.characters[i].alive && g.characters[i].type == .N00b {
			g.avatar = i
			break
		}
	}
	g.has_world = true
}
