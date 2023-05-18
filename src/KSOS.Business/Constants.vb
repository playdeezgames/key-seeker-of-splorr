Imports System.Runtime.CompilerServices

Public Module Constants
    Private ReadOnly statisticNames As IReadOnlyDictionary(Of StatisticType, String) =
        New Dictionary(Of StatisticType, String) From
        {
            {StatisticType.MaximumHealth, "Maximum Health"}
        }
    <Extension>
    Public Function Name(statisticType As StatisticType) As String
        Return statisticNames(statisticType)
    End Function
End Module
