Friend MustInherit Class SpawnerDataClient
    Inherits FeatureDataClient
    Protected Sub New(data As WorldData, featureId As Integer)
        MyBase.New(data, featureId)
    End Sub
    Protected ReadOnly Property SpawnerData As SpawnerData
        Get
            Return FeatureData.Spawner
        End Get
    End Property
End Class
