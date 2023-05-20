Friend MustInherit Class LootDataClient
    Inherits FeatureDataClient
    Protected Sub New(data As WorldData, featureId As Integer)
        MyBase.New(data, featureId)
    End Sub
    Protected ReadOnly Property LootData As LootData
        Get
            Return FeatureData.Loot
        End Get
    End Property
End Class
