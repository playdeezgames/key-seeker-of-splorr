Public Interface IItem
    Inherits IThingie
    ReadOnly Property ItemType As ItemType
    Property Quantity As Integer
    ReadOnly Property Stacks As Boolean
    ReadOnly Property Name As String
End Interface
