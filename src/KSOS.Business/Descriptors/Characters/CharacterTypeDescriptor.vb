Friend Class CharacterTypeDescriptor
    Public Property Name As String
    Public Property Statistics As IReadOnlyDictionary(Of StatisticType, Integer)
    Public Property SpawnCount As Integer
    Public Property SpawnLocations As IReadOnlyList(Of LocationType)
    Public Property KillVerb As String
    Public Property Provision As Action(Of ICharacter)
End Class
