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
        character.AddItem(character.World.CreateItem(ItemType.Dagger, 1))
        'start with nothing!
    End Sub
End Module
