Friend Module TownInitializer
    Const Columns = 5
    Const Rows = 5
    Friend Sub Initialize(world As IWorld, forestCenter As ILocation)
        Dim maze As New Maze(Of Direction)(Columns, Rows, mazeDirections)
        maze.Generate()
        Dim mazeLocations(Columns - 1, Rows - 1) As ILocation
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                mazeLocations(column, row) = world.CreateLocation()
            Next
        Next
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                Dim cell = maze.GetCell(column, row)
                For Each direction In cell.Directions
                    If cell.GetDoor(direction).Open Then
                        Dim nextColumn = CInt(mazeDirections(direction).DeltaX) + column
                        Dim nextRow = CInt(mazeDirections(direction).DeltaY) + row
                        mazeLocations(column, row).CreateRoute(direction, mazeLocations(nextColumn, nextRow))
                    End If
                Next
            Next
        Next
        mazeLocations(Columns \ 2, 0).CreateRoute(Direction.Outside, forestCenter)
        mazeLocations(Columns \ 2, Rows - 1).CreateRoute(Direction.Outside, forestCenter)
        mazeLocations(0, Rows \ 2).CreateRoute(Direction.Outside, forestCenter)
        mazeLocations(Columns - 1, Rows \ 2).CreateRoute(Direction.Outside, forestCenter)
        forestCenter.CreateRoute(Direction.Inside, mazeLocations(Columns \ 2, Rows - 1))
        Dim character As ICharacter = world.CreateCharacter(mazeLocations(RNG.FromRange(0, Columns - 1), RNG.FromRange(0, Rows - 1)))
        world.SetAvatar(character)
    End Sub
End Module
