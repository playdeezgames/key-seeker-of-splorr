Public Interface ITrade
    ReadOnly Property FromItemType As String
    ReadOnly Property FromQuantity As Integer
    ReadOnly Property ToItemType As String
    ReadOnly Property ToQuantity As Integer
    Sub Complete()
End Interface
