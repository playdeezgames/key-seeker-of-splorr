Friend Class Item
    Inherits ItemDataClient
    Implements IItem

    Public Sub New(data As WorldData, itemId As Integer)
        MyBase.New(data, itemId)
        Id = itemId
    End Sub

    Public ReadOnly Property ItemType As ItemType Implements IItem.ItemType
        Get
            Return ItemData.ItemType
        End Get
    End Property

    Public ReadOnly Property Id As Integer Implements IThingie.Id

    Public Property Quantity As Integer Implements IItem.Quantity
        Get
            Return ItemData.Quantity
        End Get
        Set(value As Integer)
            ItemData.Quantity = value
        End Set
    End Property

    Public ReadOnly Property Stacks As Boolean Implements IItem.Stacks
        Get
            Return ItemType.Descriptor.Stacks
        End Get
    End Property
End Class
