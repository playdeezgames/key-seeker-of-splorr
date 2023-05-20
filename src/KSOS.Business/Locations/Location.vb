Friend Class Location
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

    Public ReadOnly Property LocationType As LocationType Implements ILocation.LocationType
        Get
            Return LocationData.LocationType
        End Get
    End Property

    Public ReadOnly Property HasRoute(direction As Direction) As Boolean Implements ILocation.HasRoute
        Get
            Return LocationData.Routes.ContainsKey(direction)
        End Get
    End Property
    Public Sub AddCharacter(character As ICharacter) Implements ILocation.AddCharacter
        LocationData.CharacterIds.Add(character.Id)
    End Sub
    Public Sub RemoveCharacter(character As ICharacter) Implements ILocation.RemoveCharacter
        LocationData.CharacterIds.Remove(character.Id)
    End Sub
    Public ReadOnly Property Items As IEnumerable(Of IItem) Implements ILocation.Items
        Get
            Return LocationData.ItemIds.Select(Function(x) New Item(WorldData, x))
        End Get
    End Property
    Public ReadOnly Property HasItems As Boolean Implements ILocation.HasItems
        Get
            Return LocationData.ItemIds.Any
        End Get
    End Property
    Public ReadOnly Property Features As IEnumerable(Of IFeature) Implements ILocation.Features
        Get
            Return LocationData.FeatureIds.Select(Function(x) New Feature(WorldData, x))
        End Get
    End Property

    Public ReadOnly Property HasFeatures As Boolean Implements ILocation.HasFeatures
        Get
            Return LocationData.FeatureIds.Any
        End Get
    End Property

    Public Sub AddItem(item As IItem) Implements ILocation.AddItem
        If item.Stacks Then
            Dim existingItem = Items.FirstOrDefault(Function(x) x.ItemType = item.ItemType)
            If existingItem IsNot Nothing Then
                existingItem.Quantity += item.Quantity
                item.Quantity = 0
            Else
                LocationData.ItemIds.Add(item.Id)
            End If
        Else
            LocationData.ItemIds.Add(item.Id)
        End If
    End Sub
    Public Sub RemoveItem(item As IItem) Implements ILocation.RemoveItem
        LocationData.ItemIds.Remove(item.Id)
    End Sub
    Public Function CreateRoute(direction As Direction, routeType As RouteType, destination As ILocation) As IRoute Implements ILocation.CreateRoute
        LocationData.Routes(direction) = New RouteData With
            {
                .ToLocationId = destination.Id,
                .RouteType = routeType,
                .RequiredItemType = Nothing
            }
        Return New Route(WorldData, Id, direction)
    End Function
    Public Function GetRoute(direction As Direction) As IRoute Implements ILocation.GetRoute
        If LocationData.Routes.ContainsKey(direction) Then
            Return New Route(WorldData, Id, direction)
        End If
        Return Nothing
    End Function
    Public Function Enemies(character As ICharacter) As IEnumerable(Of ICharacter) Implements ILocation.Enemies
        Return LocationData.CharacterIds.Where(Function(x) x <> character.Id).Select(Function(x) New Character(WorldData, x))
    End Function
    Public Sub AddFeature(feature As IFeature) Implements ILocation.AddFeature
        LocationData.FeatureIds.Add(feature.Id)
    End Sub

    Public Sub RemoveFeature(feature As IFeature) Implements ILocation.RemoveFeature
        LocationData.FeatureIds.Remove(feature.Id)
    End Sub
End Class
