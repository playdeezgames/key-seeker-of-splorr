Public Interface ILoot
    Sub SetLoot(itemType As String, quantity As Integer)
    Sub DoLoot(character As ICharacter)
End Interface
