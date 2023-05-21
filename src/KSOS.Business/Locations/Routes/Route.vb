Friend Class Route
    Inherits RouteDataClient
    Implements IRoute
    Public Sub New(data As WorldData, locationId As Integer, direction As String)
        MyBase.New(data, locationId, direction)
        Me.Direction = direction
        Me.LocationId = locationId
    End Sub
    Public ReadOnly Property Direction As String Implements IRoute.Direction
    Private ReadOnly Property LocationId As Integer

    Public ReadOnly Property Destination As ILocation Implements IRoute.Destination
        Get
            Return New Location(WorldData, RouteData.ToLocationId)
        End Get
    End Property
    Public ReadOnly Property RouteType As String Implements IRoute.RouteType
        Get
            Return RouteData.RouteType
        End Get
    End Property

    Public Property RequiredItemType As String Implements IRoute.RequiredItemType
        Get
            Return RouteData.RequiredItemType
        End Get
        Set(value As String)
            RouteData.RequiredItemType = value
        End Set
    End Property

    Public Property SingleUse As Boolean Implements IRoute.SingleUse
        Get
            Return RouteData.SingleUse
        End Get
        Set(value As Boolean)
            RouteData.SingleUse = value
        End Set
    End Property

    Public ReadOnly Property Location As ILocation Implements IRoute.Location
        Get
            Return New Location(WorldData, LocationId)
        End Get
    End Property
End Class
