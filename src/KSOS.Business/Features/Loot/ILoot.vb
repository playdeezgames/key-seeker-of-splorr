Public Interface ILoot
    Sub SetLoot(itemType As ItemType, quantity As Integer)
    Sub DoLoot(character As ICharacter)
End Interface
