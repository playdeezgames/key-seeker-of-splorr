Friend Class Character
    Inherits CharacterDataClient
    Implements ICharacter
    Public Sub New(data As WorldData, characterId As Integer)
        MyBase.New(data, characterId)
        Id = characterId
    End Sub
    Public ReadOnly Property Id As Integer Implements IThingie.Id
    Public Property Location As ILocation Implements ICharacter.Location
        Get
            Return New Location(WorldData, CharacterData.LocationId)
        End Get
        Set(value As ILocation)
            If value.Id <> CharacterData.LocationId Then
                Location.RemoveCharacter(Me)
                CharacterData.LocationId = value.Id
                Location.AddCharacter(Me)
            End If
        End Set
    End Property
End Class
