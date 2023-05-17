Public Interface ITrade
    ReadOnly Property FromItemType As ItemType
    ReadOnly Property FromQuantity As Integer
    ReadOnly Property ToItemType As ItemType
    ReadOnly Property ToQuantity As Integer
    Sub Complete()
End Interface
