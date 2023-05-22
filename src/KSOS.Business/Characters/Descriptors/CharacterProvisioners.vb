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
        'start with nothing!
    End Sub
    Friend Sub ProvisionMummy(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.Jools, RNG.RollDice("4d6")))
    End Sub
    Friend Sub ProvisionGargoyle(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.MarbleChunk, RNG.RollDice("2d3")))
    End Sub
    Friend Sub ProvisionGuardian(character As ICharacter)
        'NOTHING! the guardian has no need of items!
    End Sub
    Friend Sub ProvisionMoonPerson(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.EmptyBottle, 1))
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
