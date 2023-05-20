Friend Module AvatarInitializer
    Friend Sub Initialize(world As IWorld)
        Dim character As ICharacter = world.Characters.Single(Function(x) x.CharacterType = CharacterType.N00b)
        world.SetAvatar(character)
    End Sub
End Module
