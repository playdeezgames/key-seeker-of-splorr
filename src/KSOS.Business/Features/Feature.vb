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

    Public ReadOnly Property FeatureType As FeatureType Implements IFeature.FeatureType
        Get
            Return FeatureData.FeatureType
        End Get
    End Property

    Public ReadOnly Property Shoppe As IShoppe Implements IFeature.Shoppe
        Get
            Return New Shoppe(WorldData, Id)
        End Get
    End Property

    Public ReadOnly Property Training As ITraining Implements IFeature.Training
        Get
            Return New Training(WorldData, Id)
        End Get
    End Property
End Class
