Public Interface IRoute
    ReadOnly Property Direction As Direction
    ReadOnly Property Destination As ILocation
    ReadOnly Property RouteType As String
    Property RequiredItemType As String
End Interface
