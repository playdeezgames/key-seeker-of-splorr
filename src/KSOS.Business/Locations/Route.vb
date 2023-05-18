Friend Class Route
    Inherits RouteDataClient
    Implements IRoute
    Public Sub New(data As WorldData, locationId As Integer, direction As Direction)
        MyBase.New(data, locationId, direction)
        Me.Direction = direction
    End Sub
    Public ReadOnly Property Direction As Direction Implements IRoute.Direction

    Public ReadOnly Property Destination As ILocation Implements IRoute.Destination
        Get
            Return New Location(WorldData, RouteData.ToLocationId)
        End Get
    End Property
    Public ReadOnly Property RouteType As RouteType Implements IRoute.RouteType
        Get
            Return RouteData.RouteType
        End Get
    End Property

    Public Property RequiredItemType As ItemType? Implements IRoute.RequiredItemType
        Get
            Return RouteData.RequiredItemType
        End Get
        Set(value As ItemType?)
            RouteData.RequiredItemType = value
        End Set
    End Property
End Class
