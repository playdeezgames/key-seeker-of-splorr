Public Interface ILocation
    Inherits IThingie
    ReadOnly Property LocationType As LocationType
    ReadOnly Property HasRoutes As Boolean
    ReadOnly Property HasRoute(direction As Direction) As Boolean
    ReadOnly Property Routes As IEnumerable(Of IRoute)
    Function GetRoute(direction As Direction) As IRoute
    Sub AddCharacter(character As ICharacter)
    Sub RemoveCharacter(character As ICharacter)
    Function CreateRoute(direction As Direction, routeType As RouteType, destination As ILocation) As IRoute
    Function Enemies(character As ICharacter) As IEnumerable(Of ICharacter)
End Interface
