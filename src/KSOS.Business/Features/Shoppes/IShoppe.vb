Public Interface IShoppe
    Property Name As String
    ReadOnly Property Trades As IEnumerable(Of ITrade)
    Sub AddTrade(tradeFrom As (String, Integer), tradeTo As (String, Integer), Optional available As Integer = Integer.MaxValue)
End Interface
