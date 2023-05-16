Public Class LocationData
    Public Property CharacterIds As New HashSet(Of Integer)
    Public Property Routes As New Dictionary(Of Direction, RouteData)
    Public Property LocationType As LocationType
    Public Property ItemIds As New HashSet(Of Integer)
End Class
