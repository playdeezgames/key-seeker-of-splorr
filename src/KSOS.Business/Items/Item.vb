Friend Class Item
    Inherits ItemDataClient
    Implements IItem
    Public Sub New(data As WorldData, itemId As Integer)
        MyBase.New(data, itemId)
        Id = itemId
    End Sub
    Public ReadOnly Property ItemType As String Implements IItem.ItemType
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
            Return ItemType.ItemTypeDescriptor.Stacks
        End Get
    End Property
    Public ReadOnly Property Name As String Implements IItem.Name
        Get
            Return ItemType.ItemTypeDescriptor.Name
        End Get
    End Property
    Public ReadOnly Property CanHeal As Boolean Implements IItem.CanHeal
        Get
            Return ItemType.ItemTypeDescriptor.Statistics.ContainsKey(StatisticType.Healing)
        End Get
    End Property
    Public ReadOnly Property CanEquip As Boolean Implements IItem.CanEquip
        Get
            Return Not String.IsNullOrEmpty(ItemType.ItemTypeDescriptor.EquipSlot)
        End Get
    End Property

    Public ReadOnly Property IsWeapon As Boolean Implements IItem.IsWeapon
        Get
            Return ItemType.ItemTypeDescriptor.Statistics.ContainsKey(StatisticType.Attack)
        End Get
    End Property

    Public ReadOnly Property IsArmor As Boolean Implements IItem.IsArmor
        Get
            Return ItemType.ItemTypeDescriptor.Statistics.ContainsKey(StatisticType.Defend)
        End Get
    End Property

    Public ReadOnly Property IsBroken As Boolean Implements IItem.IsBroken
        Get
            Return Durability = 0
        End Get
    End Property

    Public ReadOnly Property Durability As Integer Implements IItem.Durability
        Get
            Return Math.Clamp(MaximumDurability - Wear, 0, MaximumDurability)
        End Get
    End Property

    Public Property Wear As Integer
        Get
            Return Math.Clamp(GetStatistic(StatisticType.Wear), 0, MaximumDurability)
        End Get
        Set(value As Integer)
            SetStatistic(StatisticType.Wear, Math.Clamp(value, 0, MaximumDurability))
        End Set
    End Property

    Private Sub SetStatistic(statisticType As StatisticType, value As Integer)
        ItemData.Statistics(statisticType) = value
    End Sub

    Private Function GetStatistic(statisticType As StatisticType) As Integer
        If ItemData.Statistics.ContainsKey(statisticType) Then
            Return ItemData.Statistics(statisticType)
        End If
        Return ItemType.ItemTypeDescriptor.Statistics(statisticType)
    End Function

    Public ReadOnly Property MaximumDurability As Integer Implements IItem.MaximumDurability
        Get
            Return GetStatistic(StatisticType.Durability)
        End Get
    End Property

    Public ReadOnly Property MaximumAttack As Integer Implements IItem.MaximumAttack
        Get
            Return GetStatistic(StatisticType.MaximumAttack)
        End Get
    End Property

    Public ReadOnly Property MaximumDefend As Integer Implements IItem.MaximumDefend
        Get
            Return GetStatistic(StatisticType.MaximumDefend)
        End Get
    End Property

    Public ReadOnly Property Attack As Integer Implements IItem.Attack
        Get
            Return GetStatistic(StatisticType.Attack)
        End Get
    End Property

    Public ReadOnly Property Defend As Integer Implements IItem.Defend
        Get
            Return GetStatistic(StatisticType.Defend)
        End Get
    End Property

    Public Sub DoWear(wear As Integer) Implements IItem.DoWear
        Me.Wear += wear
    End Sub
End Class
