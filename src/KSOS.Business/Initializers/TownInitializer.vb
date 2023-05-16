﻿Friend Module TownInitializer
    Const Columns = 5
    Const Rows = 5
    Friend Sub Initialize(world As IWorld)
        Dim maze As New Maze(Of Direction)(Columns, Rows, mazeDirections)
        maze.Generate()
        Dim mazeLocations(Columns - 1, Rows - 1) As ILocation
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                Dim lt = If(
                    column = 0 OrElse column = Columns - 1 OrElse row = 0 OrElse row = Rows - 1,
                    LocationType.TownEdge,
                    LocationType.Town)
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
                        mazeLocations(column, row).CreateRoute(direction, RouteType.DirtRoad, mazeLocations(nextColumn, nextRow))
                    End If
                Next
            Next
        Next
        Dim forestCenter = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.ForestCenter))
        mazeLocations(Columns \ 2, 0).CreateRoute(Direction.North, RouteType.TownGate, forestCenter)
        mazeLocations(Columns \ 2, Rows - 1).CreateRoute(Direction.South, RouteType.TownGate, forestCenter)
        mazeLocations(0, Rows \ 2).CreateRoute(Direction.West, RouteType.TownGate, forestCenter)
        mazeLocations(Columns - 1, Rows \ 2).CreateRoute(Direction.East, RouteType.TownGate, forestCenter)
        forestCenter.CreateRoute(Direction.Inside, RouteType.TownGate, mazeLocations(Columns \ 2, Rows - 1))
        InitializeInn(world)
    End Sub

    Private Sub InitializeInn(world As IWorld)
        Dim entrance = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.TownEdge AndAlso Not x.HasRoute(Direction.Inside)))
        Dim inn = world.CreateLocation(LocationType.Inn)
        entrance.CreateRoute(Direction.Inside, RouteType.Door, inn)
        inn.CreateRoute(Direction.Outside, RouteType.Door, entrance)
        Dim cellar = world.CreateLocation(LocationType.Cellar)
        inn.CreateRoute(Direction.Down, RouteType.Stairs, cellar)
        cellar.CreateRoute(Direction.Up, RouteType.Stairs, inn)
        Dim gurachan = world.CreateFeature("Gurachan")
        inn.AddFeature(gurachan)
    End Sub
End Module
