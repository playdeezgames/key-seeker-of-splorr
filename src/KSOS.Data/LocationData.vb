Public Class LocationData
    Property CharacterIds As New HashSet(Of Integer)
    Property Routes As New Dictionary(Of Direction, RouteData)
    Property LocationType As LocationType
End Class
