Public MustInherit Class WorldDataClient
    Private ReadOnly _data As WorldData
    Protected ReadOnly Property WorldData As WorldData
        Get
            Return _data
        End Get
    End Property
    Protected Sub New(data As WorldData)
        _data = data
    End Sub
End Class
