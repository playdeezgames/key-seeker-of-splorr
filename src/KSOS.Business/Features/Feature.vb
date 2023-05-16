Friend Class Feature
    Inherits FeatureDataClient
    Implements IFeature
    Sub New(data As WorldData, featureId As Integer)
        MyBase.New(data, featureId)
        Id = featureId
    End Sub
    Public ReadOnly Property Id As Integer Implements IThingie.Id

    Public ReadOnly Property Name As String Implements IFeature.Name
        Get
            Return FeatureData.Name
        End Get
    End Property
End Class
