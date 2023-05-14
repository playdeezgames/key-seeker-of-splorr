Imports System.Security.Cryptography.X509Certificates

Friend Module TownInitializer
    Const Columns = 5
    Const Rows = 5
    ReadOnly mazeDirections As IReadOnlyDictionary(Of Direction, MazeDirection(Of Direction)) =
        New Dictionary(Of Direction, MazeDirection(Of Direction)) From
        {
            {Direction.North, New MazeDirection(Of Direction)(Direction.South, 0, -1)},
            {Direction.East, New MazeDirection(Of Direction)(Direction.West, 1, 0)},
            {Direction.South, New MazeDirection(Of Direction)(Direction.North, 0, 1)},
            {Direction.West, New MazeDirection(Of Direction)(Direction.East, -1, 0)}
        }
    Friend Sub Initialize(world As IWorld)
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
        Dim character As ICharacter = world.CreateCharacter(mazeLocations(RNG.FromRange(0, Columns - 1), RNG.FromRange(0, Rows - 1)))
        world.SetAvatar(character)
    End Sub
End Module
