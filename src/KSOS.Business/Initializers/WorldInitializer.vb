Friend Module WorldInitializer
    Friend ReadOnly mazeDirections As IReadOnlyDictionary(Of Direction, MazeDirection(Of Direction)) =
        New Dictionary(Of Direction, MazeDirection(Of Direction)) From
        {
            {Direction.North, New MazeDirection(Of Direction)(Direction.South, 0, -1)},
            {Direction.East, New MazeDirection(Of Direction)(Direction.West, 1, 0)},
            {Direction.South, New MazeDirection(Of Direction)(Direction.North, 0, 1)},
            {Direction.West, New MazeDirection(Of Direction)(Direction.East, -1, 0)}
        }
    Friend Sub Initialize(world As IWorld)
        Dim forestCenter = ForestInitializer.Initialize(world)
        TownInitializer.Initialize(world, forestCenter)
        GraveyardInitializer.Initialize(world)

        'Dim spawnLocation = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.ForestCorner AndAlso x.Routes.Any(Function(y) y.RouteType = RouteType.GraveyardGate)))
        Dim spawnLocation = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.Town))
        Dim character As ICharacter = world.CreateCharacter(spawnLocation)
        world.SetAvatar(character)
    End Sub
End Module
