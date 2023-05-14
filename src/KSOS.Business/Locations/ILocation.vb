Public Interface ILocation
    Inherits IThingie
    ReadOnly Property HasRoutes As Boolean
    ReadOnly Property Routes As IEnumerable(Of IRoute)
    Function GetRoute(direction As Direction) As IRoute
    Sub AddCharacter(character As ICharacter)
    Sub RemoveCharacter(character As ICharacter)
    Function CreateRoute(direction As Direction, destination As ILocation) As IRoute
End Interface
