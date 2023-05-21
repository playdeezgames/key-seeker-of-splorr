Friend Class TrainingStatistic
    Inherits TrainingDataClient
    Implements ITrainingStatistic
    Public Sub New(worldData As WorldData, featureId As Integer, statisticType As String)
        MyBase.New(worldData, featureId)
        Me.StatisticType = statisticType
    End Sub

    Public ReadOnly Property StatisticType As String Implements ITrainingStatistic.StatisticType

    Public ReadOnly Property Multiplier As Integer Implements ITrainingStatistic.Multiplier
        Get
            Return TrainingData.Statistics(StatisticType)
        End Get
    End Property

    Public Function CalculateCost(character As ICharacter) As Integer Implements ITrainingStatistic.CalculateCost
        Return Multiplier * character.GetStatistic(StatisticType)
    End Function
End Class
