﻿Friend Class Route
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
End Class
