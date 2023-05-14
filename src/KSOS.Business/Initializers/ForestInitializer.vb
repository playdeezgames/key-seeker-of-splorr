Friend Module ForestInitializer
    Const Columns = 11
    Const Rows = 11
    Friend Function Initialize(world As IWorld) As ILocation
        Dim maze As New Maze(Of Direction)(Columns, Rows, mazeDirections)
        maze.Generate()
        Dim mazeLocations(Columns - 1, Rows - 1) As ILocation
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                mazeLocations(column, row) = world.CreateLocation()
            Next
        Next
        Return mazeLocations(Columns \ 2, Rows \ 2)
    End Function
End Module
