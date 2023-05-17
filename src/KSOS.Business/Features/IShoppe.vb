Public Interface IShoppe
    Property Name As String
    ReadOnly Property Trades As IEnumerable(Of ITrade)
    Sub AddTrade(tradeFrom As (ItemType, Integer), tradeTo As (ItemType, Integer), Optional available As Integer? = Nothing)
End Interface
