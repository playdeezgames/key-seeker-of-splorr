Friend Class Route
    Inherits RouteDataClient
    Implements IRoute
    Public Sub New(data As WorldData, locationId As Integer, direction As Direction)
        MyBase.New(data, locationId, direction)
    End Sub
End Class
