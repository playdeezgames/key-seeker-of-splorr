Public Interface ITrainingStatistic
    ReadOnly Property StatisticType As StatisticType
    ReadOnly Property Multiplier As Integer
    Function CalculateCost(character As ICharacter) As Integer
End Interface
