Public Class World
    Inherits WorldDataClient
    Implements IWorld
    Public ReadOnly Property Avatar As ICharacter Implements IWorld.Avatar
        Get
            If WorldData.CharacterIndex.HasValue Then
                Return New Character(WorldData, WorldData.CharacterIndex.Value)
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property Locations As IEnumerable(Of ILocation) Implements IWorld.Locations
        Get
            Dim result As New List(Of ILocation)
            Dim index = 0
            For Each location In WorldData.Locations
                If location IsNot Nothing Then
                    result.Add(New Location(WorldData, index))
                End If
                index += 1
            Next
            Return result
        End Get
    End Property

    Public Sub New(data As WorldData)
        MyBase.New(data)
    End Sub
    Public Sub New()
        MyBase.New(New WorldData)
        WorldInitializer.Initialize(Me)
    End Sub
    Public Sub SetAvatar(character As ICharacter) Implements IWorld.SetAvatar
        WorldData.CharacterIndex = character?.Id
    End Sub
    Public Function CreateLocation(locationType As LocationType) As ILocation Implements IWorld.CreateLocation
        Dim locationId = WorldData.Locations.Count
        WorldData.Locations.Add(New LocationData With
                                {
                                    .LocationType = locationType
                                })
        Return New Location(WorldData, locationId)
    End Function
    Public Function CreateCharacter(location As ILocation) As ICharacter Implements IWorld.CreateCharacter
        Dim characterId = WorldData.Characters.Count
        WorldData.Characters.Add(New CharacterData With {.LocationId = location.Id})
        Dim character = New Character(WorldData, characterId)
        location.AddCharacter(character)
        Return character
    End Function

    Public Function Save(fileName As String) As Boolean Implements IWorld.Save
        Try
            File.WriteAllText(fileName, JsonSerializer.Serialize(WorldData))
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function Load(fileName As String) As IWorld
        Try
            Return New World(JsonSerializer.Deserialize(Of WorldData)(File.ReadAllText(fileName)))
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class
