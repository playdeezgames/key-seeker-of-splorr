Friend MustInherit Class ShoppeDataClient
    Inherits FeatureDataClient

    Protected Sub New(data As WorldData, featureId As Integer)
        MyBase.New(data, featureId)
    End Sub
    Protected ReadOnly Property ShoppeData As ShoppeData
        Get
            Return FeatureData.Shoppe
        End Get
    End Property
End Class
