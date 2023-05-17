Friend MustInherit Class TradeDataClient
    Inherits ShoppeDataClient
    Protected ReadOnly Property TradeData As TradeData
        Get
            Return ShoppeData.Trades(_tradeIndex)
        End Get
    End Property
    Private _tradeIndex As Integer
    Protected Sub New(data As WorldData, featureId As Integer, tradeIndex As Integer)
        MyBase.New(data, featureId)
        _tradeIndex = tradeIndex
    End Sub
End Class
