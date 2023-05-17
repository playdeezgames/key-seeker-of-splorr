Friend Class Shoppe
    Inherits ShoppeDataClient
    Implements IShoppe
    ReadOnly Property FeatureId As Integer

    Sub New(data As WorldData, featureId As Integer)
        MyBase.New(data, featureId)
        Me.FeatureId = featureId
    End Sub

    Public Property Name As String Implements IShoppe.Name
        Get
            Return ShoppeData.Name
        End Get
        Set(value As String)
            ShoppeData.Name = value
        End Set
    End Property

    Public ReadOnly Property Trades As IEnumerable(Of ITrade) Implements IShoppe.Trades
        Get
            Dim result As New List(Of ITrade)
            For index = 0 To ShoppeData.Trades.Count - 1
                Dim trade = ShoppeData.Trades(index)
                If trade.Available <> 0 Then
                    result.Add(New Trade(WorldData, FeatureId, index))
                End If
            Next
            Return result
        End Get
    End Property

    Public Sub AddTrade(tradeFrom As (ItemType, Integer), tradeTo As (ItemType, Integer), Optional available As Integer? = Nothing) Implements IShoppe.AddTrade
        ShoppeData.Trades.Add(New TradeData With {
            .Available = available,
            .FromItemType = tradeFrom.Item1,
            .FromQuantity = tradeFrom.Item2,
            .ToItemType = tradeTo.Item1,
            .ToQuantity = tradeTo.Item2
            })
    End Sub
End Class
