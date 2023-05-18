Friend Class Training
    Inherits TrainingDataClient
    Implements ITraining
    Private ReadOnly _featureId As Integer
    Public Sub New(data As WorldData, featureId As Integer)
        MyBase.New(data, featureId)
        _featureId = featureId
    End Sub

    Public Property Name As String Implements ITraining.Name
        Get
            Return TrainingData.Name
        End Get
        Set(value As String)
            TrainingData.Name = value
        End Set
    End Property

    Public ReadOnly Property Statistics As IEnumerable(Of ITrainingStatistic) Implements ITraining.Statistics
        Get
            Return TrainingData.Statistics.Select(Function(x) New TrainingStatistic(WorldData, _featureId, x.Key))
        End Get
    End Property

    Public Sub Add(statisticType As StatisticType, multiplier As Integer) Implements ITraining.Add
        TrainingData.Statistics(statisticType) = multiplier
    End Sub
End Class
