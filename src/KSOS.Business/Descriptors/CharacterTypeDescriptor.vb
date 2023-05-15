Friend Class CharacterTypeDescriptor
    Public Property Name As String
    Public Property Statistics As IReadOnlyDictionary(Of StatisticType, Integer)
    Public Property SpawnCount As Integer
    Public Property SpawnLocations As IReadOnlyList(Of LocationType)
End Class
