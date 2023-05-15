Friend Module GraveyardInitializer
    Const Columns = 7
    Const Rows = 7
    Friend Sub Initialize(world As IWorld)
        Dim locations(Columns - 1, Rows - 1) As ILocation
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                locations(column, row) = world.CreateLocation(LocationType.Graveyard)
            Next
        Next
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                If row > 0 Then
                    locations(column, row).CreateRoute(Direction.North, RouteType.GraveyardPath, locations(column, row - 1))
                End If
                If column > 0 Then
                    locations(column, row).CreateRoute(Direction.West, RouteType.GraveyardPath, locations(column - 1, row))
                End If
                If row < Rows - 1 Then
                    locations(column, row).CreateRoute(Direction.South, RouteType.GraveyardPath, locations(column, row + 1))
                End If
                If column < Columns - 1 Then
                    locations(column, row).CreateRoute(Direction.East, RouteType.GraveyardPath, locations(column + 1, row))
                End If
            Next
        Next
        Dim forestLocation = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.ForestCorner AndAlso Not x.HasRoute(Direction.Inside)))
        forestLocation.CreateRoute(Direction.Inside, RouteType.GraveyardGate, locations(Columns \ 2, Rows - 1))
        locations(Columns \ 2, Rows - 1).CreateRoute(Direction.South, RouteType.GraveyardGate, forestLocation)
    End Sub
End Module
