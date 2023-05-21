Public Interface IRoute
    ReadOnly Property Direction As String
    ReadOnly Property Destination As ILocation
    ReadOnly Property RouteType As String
    Property RequiredItemType As String
    Property SingleUse As Boolean
    ReadOnly Property Location As ILocation
End Interface
