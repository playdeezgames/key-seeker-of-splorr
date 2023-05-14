Public MustInherit Class CharacterDataClient
    Inherits WorldDataClient
    Private ReadOnly _characterId As Integer
    Protected ReadOnly Property CharacterData As CharacterData
        Get
            Return WorldData.Characters(_characterId)
        End Get
    End Property
    Protected Sub New(data As WorldData, characterId As Integer)
        MyBase.New(data)
        _characterId = characterId
    End Sub
End Class
