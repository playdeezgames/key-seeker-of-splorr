Friend Module WorldInitializer
    Friend ReadOnly mazeDirections As IReadOnlyDictionary(Of String, MazeDirection(Of String)) =
        New Dictionary(Of String, MazeDirection(Of String)) From
        {
            {Direction.North, New MazeDirection(Of String)(Direction.South, 0, -1)},
            {Direction.East, New MazeDirection(Of String)(Direction.West, 1, 0)},
            {Direction.South, New MazeDirection(Of String)(Direction.North, 0, 1)},
            {Direction.West, New MazeDirection(Of String)(Direction.East, -1, 0)}
        }
    Friend Sub Initialize(world As IWorld)
        ForestInitializer.Initialize(world)
        TownInitializer.Initialize(world)
        GraveyardInitializer.Initialize(world)
        RuinsInitializer.Initialize(world)
        SewersInitializer.Initialize(world)
        TowerInitializer.Initialize(world)

        CharacterInitializer.Initialize(world)
        AvatarInitializer.Initialize(world)
    End Sub
End Module
