Public MustInherit Class ItemDataClient
    Inherits WorldDataClient
    Private ReadOnly _itemId As Integer
    Protected ReadOnly Property ItemData As ItemData
        Get
            Return WorldData.Items(_itemId)
        End Get
    End Property
    Protected Sub New(data As WorldData, itemId As Integer)
        MyBase.New(data)
        _itemId = itemId
    End Sub
End Class
