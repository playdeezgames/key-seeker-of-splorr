Friend Module WorldInitializer
    Friend Sub Initialize(world As IWorld)
        Dim location As ILocation = world.CreateLocation()
        Dim character As ICharacter = world.CreateCharacter(location)
        world.SetAvatar(character)
    End Sub
End Module
