Imports System.Reflection.Emit

Friend Module UfoInitializer
    Const Columns = 5
    Const Rows = 5
    Friend Sub Initialize(world As IWorld)
        Dim maze As New Maze(Of String)(Columns, Rows, mazeDirections)
        maze.Generate()
        Dim mazeLocations(Columns - 1, Rows - 1) As ILocation
        Dim column As Integer
        Dim row As Integer
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                Dim lt = LocationType.Ufo
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
                        mazeLocations(column, row).CreateRoute(direction, RouteType.Hallway, mazeLocations(nextColumn, nextRow))
                    End If
                Next
            Next
        Next
        Dim forestLocation = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.ForestCorner AndAlso Not x.HasRoute(Direction.Inside)))
        Dim graveyardEntrance = forestLocation.CreateRoute(Direction.Up, RouteType.UfoRamp, mazeLocations(Columns \ 2, Rows \ 2))
        graveyardEntrance.RequiredItemType = ItemType.UfoKey
        mazeLocations(Columns \ 2, Rows \ 2).CreateRoute(Direction.Down, RouteType.UfoRamp, forestLocation)
        column = RNG.FromRange(0, Columns - 1)
        row = RNG.FromRange(0, Rows - 1)
        Dim feature = world.CreateFeature("Metal Box", FeatureType.Loot)
        feature.Loot.SetLoot(ItemType.MachineKey, 1)
        mazeLocations(column, row).AddFeature(feature)
    End Sub
End Module
