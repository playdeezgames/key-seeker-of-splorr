Friend Module RuinsInitializer
    Const Columns = 6
    Const Rows = 6
    Friend Sub Initialize(world As IWorld)
        Dim locations(Columns - 1, Rows - 1) As ILocation
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                locations(column, row) = world.CreateLocation(LocationType.Ruins)
            Next
        Next
        Dim maze As New Maze(Of String)(Columns, Rows, mazeDirections)
        maze.Generate()
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                Dim cell = maze.GetCell(column, row)
                For Each direction In cell.Directions
                    If cell.GetDoor(direction).Open Then
                        Dim nextLocation = locations(column + CInt(mazeDirections(direction).DeltaX), row + CInt(mazeDirections(direction).DeltaY))
                        locations(column, row).CreateRoute(direction, RouteType.RuinsPath, nextLocation)
                    End If
                Next
            Next
        Next
        Dim forestLocation = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.ForestCorner AndAlso Not x.HasRoute(Direction.Inside)))
        forestLocation.CreateRoute(Direction.Inside, RouteType.RuinsEntrance, locations(0, 0))
        locations(0, 0).CreateRoute(Direction.Outside, RouteType.RuinsEntrance, forestLocation)
        InitializeMachine(world)
    End Sub

    Private Sub InitializeMachine(world As IWorld)
        Dim ruinLocation = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.Ruins))
        Dim machineLocation = world.CreateLocation(LocationType.Machine)
        Dim entrance = ruinLocation.CreateRoute(Direction.Down, RouteType.Hatch, machineLocation)
        entrance.RequiredItemType = ItemType.MachineKey
        machineLocation.CreateRoute(Direction.Up, RouteType.Hatch, ruinLocation)
        Dim feature = world.CreateFeature("lever", FeatureType.Spawner)
        feature.Spawner.SetSpawnWeight(CharacterType.Guardian, 1)
        machineLocation.AddFeature(feature)
    End Sub
End Module
