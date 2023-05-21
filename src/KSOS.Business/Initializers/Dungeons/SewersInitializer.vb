Friend Module SewersInitializer
    Const Columns = 6
    Const Rows = 6
    Friend Sub Initialize(world As IWorld)
        Dim maze As New Maze(Of String)(Columns, Rows, mazeDirections)
        maze.Generate()
        Dim mazeLocations(Columns - 1, Rows - 1) As ILocation
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                Dim lt = LocationType.Sewer
                mazeLocations(column, row) = world.CreateLocation(lt)
            Next
        Next
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                Dim cell = maze.GetCell(column, row)
                For Each direction In cell.Directions
                    If cell.GetDoor(direction).Open Then
                        Dim nextColumn = CInt(mazeDirections(direction).DeltaX) + column
                        Dim nextRow = CInt(mazeDirections(direction).DeltaY) + row
                        mazeLocations(column, row).CreateRoute(direction, RouteType.Tunnel, mazeLocations(nextColumn, nextRow))
                    End If
                Next
            Next
        Next
        Dim sewerStart = mazeLocations(RNG.FromRange(0, Columns - 1), RNG.FromRange(0, Rows - 1))
        Dim entrance = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.TownEdge AndAlso Not x.HasRoute(Direction.Down)))
        Dim stairsDown = entrance.CreateRoute(Direction.Down, RouteType.Stairs, sewerStart)
        stairsDown.RequiredItemType = ItemType.SewerKey
        sewerStart.CreateRoute(Direction.Up, RouteType.Stairs, entrance)
    End Sub
End Module
