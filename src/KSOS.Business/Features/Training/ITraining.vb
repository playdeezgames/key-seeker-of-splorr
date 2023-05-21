Public Interface ITraining
    Property Name As String
    Sub Add(statisticType As String, multiplier As Integer)
    ReadOnly Property Statistics As IEnumerable(Of ITrainingStatistic)
End Interface
