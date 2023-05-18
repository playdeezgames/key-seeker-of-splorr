Friend Class NextMessage
    Inherits WorldDataClient
    Implements IMessage

    Public Sub New(data As WorldData)
        MyBase.New(data)
    End Sub

    Public ReadOnly Property Lines As IEnumerable(Of String) Implements IMessage.Lines
        Get
            Return WorldData.Messages.First.Lines
        End Get
    End Property
End Class
