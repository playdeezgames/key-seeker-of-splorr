Friend Module WorldInitializer
    Friend Sub Initialize(world As IWorld)
        Dim startLocation As ILocation = world.CreateLocation()
        Dim character As ICharacter = world.CreateCharacter(startLocation)
        world.SetAvatar(character)
        Dim nextLocation As ILocation = world.CreateLocation()
        startLocation.CreateRoute(Direction.North, nextLocation)
        nextLocation.CreateRoute(Direction.South, startLocation)
    End Sub
End Module
