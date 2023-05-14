Public Interface ICharacter
    Inherits IThingie
    Property Location As ILocation
    Property Name As String
    Sub Move(direction As Direction)
End Interface
