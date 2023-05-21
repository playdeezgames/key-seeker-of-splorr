Friend Module CharacterProvisioners
    Friend Sub ProvisionBlob(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.Jools, RNG.RollDice("1d3")))
    End Sub
    Friend Sub ProvisionCellarRat(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.CellarRatTail, 1))
    End Sub
    Friend Sub ProvisionSewerRat(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.SewerRatTail, 1))
    End Sub
    Friend Sub ProvisionKingRat(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.SewerRatTail, 1))
        character.AddItem(character.World.CreateItem(ItemType.GraveyardKey, 1))
    End Sub
    Friend Sub ProvisionN00b(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.Chikkin, 10))
        character.AddItem(character.World.CreateItem(ItemType.Potion, 1))
        character.AddItem(character.World.CreateItem(ItemType.HolyWater, 1))
        'start with nothing!
    End Sub
    Friend Sub ProvisionSkeleton(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.SkullFragment, 1))
    End Sub
    Friend Sub ProvisionZombie(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.ZombieTaint, 1))
        If RNG.FromRange(1, 100) < 10 Then
            character.AddItem(character.World.CreateItem(ItemType.Feather, 1))
        End If
    End Sub
    Friend Sub ProvisionGoblin(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.GoblinEar, 1))
        character.AddItem(character.World.CreateItem(ItemType.Jools, RNG.RollDice("1d4")))
    End Sub
    Friend Sub ProvisionOrc(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.OrcTooth, 1))
        character.AddItem(character.World.CreateItem(ItemType.Jools, RNG.RollDice("2d4")))
    End Sub
    Friend Sub ProvisionCyclops(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.CyclopsEyeball, 1))
        character.AddItem(character.World.CreateItem(ItemType.Jools, RNG.RollDice("3d4")))
    End Sub
End Module
