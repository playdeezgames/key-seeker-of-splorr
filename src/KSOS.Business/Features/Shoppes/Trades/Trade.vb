﻿Friend Class Trade
    Inherits TradeDataClient
    Implements ITrade
    Public Sub New(data As WorldData, featureId As Integer, tradeIndex As Integer)
        MyBase.New(data, featureId, tradeIndex)
    End Sub

    Public ReadOnly Property FromItemType As String Implements ITrade.FromItemType
        Get
            Return TradeData.FromItemType
        End Get
    End Property

    Public ReadOnly Property FromQuantity As Integer Implements ITrade.FromQuantity
        Get
            Return TradeData.FromQuantity
        End Get
    End Property

    Public ReadOnly Property ToItemType As String Implements ITrade.ToItemType
        Get
            Return TradeData.ToItemType
        End Get
    End Property

    Public ReadOnly Property ToQuantity As Integer Implements ITrade.ToQuantity
        Get
            Return TradeData.ToQuantity
        End Get
    End Property

    Public Sub Complete() Implements ITrade.Complete
        If TradeData.Available < Integer.MaxValue AndAlso TradeData.Available > 0 Then
            TradeData.Available -= 1
        End If
    End Sub
End Class
