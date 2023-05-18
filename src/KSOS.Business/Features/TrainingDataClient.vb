Friend MustInherit Class TrainingDataClient
    Inherits FeatureDataClient

    Protected Sub New(data As WorldData, featureId As Integer)
        MyBase.New(data, featureId)
    End Sub
    Protected ReadOnly Property TrainingData As TrainingData
        Get
            Return FeatureData.Training
        End Get
    End Property
End Class
