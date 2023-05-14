Public Interface ILocation
    Inherits IThingie
    Sub AddCharacter(character As ICharacter)
    Sub RemoveCharacter(character As ICharacter)
    Function CreateRoute(direction As Direction, destination As ILocation) As IRoute
End Interface
