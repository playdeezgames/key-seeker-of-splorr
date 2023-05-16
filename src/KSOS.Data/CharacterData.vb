Public Class CharacterData
    Public Property CharacterType As CharacterType
    Public Property LocationId As Integer
    Public Property Name As String
    Public Property Statistics As New Dictionary(Of StatisticType, Integer)
    Public Property ItemIds As New HashSet(Of Integer)
End Class
