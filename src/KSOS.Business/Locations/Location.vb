﻿Friend Class Location
    Inherits LocationDataClient
    Implements ILocation
    Public Sub New(data As WorldData, locationId As Integer)
        MyBase.New(data, locationId)
        Id = locationId
    End Sub
    Public ReadOnly Property Id As Integer Implements IThingie.Id
    Public ReadOnly Property HasRoutes As Boolean Implements ILocation.HasRoutes
        Get
            Return LocationData.Routes.Any
        End Get
    End Property
    Public ReadOnly Property Routes As IEnumerable(Of IRoute) Implements ILocation.Routes
        Get
            Return LocationData.Routes.Keys.Select(Function(x) New Route(WorldData, Id, x))
        End Get
    End Property
    Public Sub AddCharacter(character As ICharacter) Implements ILocation.AddCharacter
        LocationData.CharacterIds.Add(character.Id)
    End Sub
    Public Sub RemoveCharacter(character As ICharacter) Implements ILocation.RemoveCharacter
        LocationData.CharacterIds.Remove(character.Id)
    End Sub
    Public Function CreateRoute(direction As Direction, destination As ILocation) As IRoute Implements ILocation.CreateRoute
        LocationData.Routes(direction) = New RouteData With {.ToLocationId = destination.Id}
        Return New Route(WorldData, Id, direction)
    End Function

    Public Function GetRoute(direction As Direction) As IRoute Implements ILocation.GetRoute
        If LocationData.Routes.ContainsKey(direction) Then
            Return New Route(WorldData, Id, direction)
        End If
        Return Nothing
    End Function
End Class
