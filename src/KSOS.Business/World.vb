Public Class World
    Inherits WorldDataClient
    Implements IWorld
    Public Sub New(data As WorldData)
        MyBase.New(data)
    End Sub
    Public Sub New()
        MyBase.New(New WorldData)
    End Sub
    Public Sub SetAvatar(character As ICharacter) Implements IWorld.SetAvatar
        WorldData.CharacterIndex = character?.Id
    End Sub
    Public Function CreateLocation() As ILocation Implements IWorld.CreateLocation
        Dim locationId = WorldData.Locations.Count
        WorldData.Locations.Add(New LocationData)
        Return New Location(WorldData, locationId)
    End Function
    Public Function CreateCharacter(location As ILocation) As ICharacter Implements IWorld.CreateCharacter
        Dim characterId = WorldData.Characters.Count
        WorldData.Characters.Add(New CharacterData With {.LocationId = location.Id})
        Dim character = New Character(WorldData, characterId)
        location.AddCharacter(character)
        Return character
    End Function
End Class
