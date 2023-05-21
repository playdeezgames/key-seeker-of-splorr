Public Class CharacterData
    Public Property CharacterType As String
    Public Property LocationId As Integer
    Public Property Name As String
    Public Property Statistics As New Dictionary(Of String, Integer)
    Public Property ItemIds As New HashSet(Of Integer)
    Public Property Equipment As New Dictionary(Of String, Integer)
End Class
