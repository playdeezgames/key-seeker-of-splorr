Friend Module ForestInitializer
    Const Columns = 11
    Const Rows = 11
    Friend Function Initialize(world As IWorld) As ILocation
        Dim maze As New Maze(Of String)(Columns, Rows, mazeDirections)
        maze.Generate()
        Dim mazeLocations(Columns - 1, Rows - 1) As ILocation
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                Dim lt = If(
                    column = Columns \ 2 AndAlso row = Rows \ 2,
                    LocationType.ForestCenter,
                    If(
                        (column = 0 OrElse column = Columns - 1) AndAlso (row = 0 OrElse row = Rows - 1),
                        LocationType.ForestCorner,
                        LocationType.Forest))
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
                        mazeLocations(column, row).CreateRoute(direction, RouteType.ForestPath, mazeLocations(nextColumn, nextRow))
                    End If
                Next
            Next
        Next
        Return mazeLocations(Columns \ 2, Rows \ 2)
    End Function
End Module
