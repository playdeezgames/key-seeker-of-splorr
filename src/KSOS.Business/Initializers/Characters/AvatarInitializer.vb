Friend Module AvatarInitializer
    Friend Sub Initialize(world As IWorld)
        'Graveyard:      Dim spawnLocation = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.ForestCorner AndAlso x.Routes.Any(Function(y) y.RouteType = RouteType.GraveyardGate)))
        'Town:
        Dim spawnLocation = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.Town))
        Dim character As ICharacter = world.CreateCharacter(CharacterType.N00b, spawnLocation)
        world.SetAvatar(character)
    End Sub
End Module
