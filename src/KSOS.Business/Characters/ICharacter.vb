Public Interface ICharacter
    Inherits IThingie
    Property Location As ILocation
    Sub Move(direction As Direction)
End Interface
