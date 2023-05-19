Public MustInherit Class RouteDataClient
    Inherits LocationDataClient
    Private ReadOnly _direction As Direction
    Protected ReadOnly Property RouteData As RouteData
        Get
            Return LocationData.Routes(_direction)
        End Get
    End Property
    Protected Sub New(data As WorldData, locationId As Integer, direction As Direction)
        MyBase.New(data, locationId)
        _direction = direction
    End Sub
End Class
