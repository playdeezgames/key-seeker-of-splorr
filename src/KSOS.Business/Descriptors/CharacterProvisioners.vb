Friend Module CharacterProvisioners
    Friend Sub ProvisionBlob(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.Jools, RNG.RollDice("1d3")))
    End Sub
    Friend Sub ProvisionRat(character As ICharacter)
        character.AddItem(character.World.CreateItem(ItemType.RatTail, 1))
    End Sub
    Friend Sub ProvisionN00b(character As ICharacter)
        'start with nothing!
    End Sub
End Module
