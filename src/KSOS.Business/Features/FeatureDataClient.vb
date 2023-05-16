Friend Class FeatureDataClient
    Inherits WorldDataClient
    Private ReadOnly _featureId As Integer
    Protected ReadOnly Property FeatureData As FeatureData
        Get
            Return WorldData.Features(_featureId)
        End Get
    End Property
    Protected Sub New(data As WorldData, featureId As Integer)
        MyBase.New(data)
        _featureId = featureId
    End Sub
End Class
