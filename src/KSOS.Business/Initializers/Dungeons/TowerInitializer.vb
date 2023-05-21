Friend Module TowerInitializer
    Const Columns = 3
    Const Rows = 3
    Const Levels = 7
    Friend Sub Initialize(world As IWorld)
        Dim locations(Levels - 1, Columns - 1, Rows - 1) As ILocation
        For level = 0 To Levels - 1
            For column = 0 To Columns - 1
                For row = 0 To Rows - 1
                    locations(level, column, row) = world.CreateLocation(LocationType.Tower)
                Next
            Next
            Dim maze As New Maze(Of String)(Columns, Rows, mazeDirections)
            maze.Generate()
            For column = 0 To Columns - 1
                For row = 0 To Rows - 1
                    Dim cell = maze.GetCell(column, row)
                    For Each direction In cell.Directions
                        If cell.GetDoor(direction).Open Then
                            Dim nextColumn = CInt(mazeDirections(direction).DeltaX) + column
                            Dim nextRow = CInt(mazeDirections(direction).DeltaY) + row
                            locations(level, column, row).CreateRoute(direction, RouteType.Door, locations(level, nextColumn, nextRow))
                        End If
                    Next
                Next
            Next
            If level > 0 Then
                Dim column = RNG.FromRange(0, Columns - 1)
                Dim row = RNG.FromRange(0, Rows - 1)
                locations(level, column, row).CreateRoute(Direction.Down, RouteType.Stairs, locations(level - 1, column, row))
                locations(level - 1, column, row).CreateRoute(Direction.Up, RouteType.Stairs, locations(level, column, row))
            End If
        Next
        Dim forestLocation = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.ForestCorner AndAlso Not x.HasRoute(Direction.Inside)))
        Dim entrance = forestLocation.CreateRoute(Direction.Inside, RouteType.TowerEntrance, locations(0, Columns \ 2, Rows - 1))
        entrance.RequiredItemType = ItemType.TowerKey
        locations(0, Columns \ 2, Rows - 1).CreateRoute(Direction.Outside, RouteType.TowerEntrance, forestLocation)
    End Sub
End Module
