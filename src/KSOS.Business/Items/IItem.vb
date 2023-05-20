Public Interface IItem
    Inherits IThingie
    ReadOnly Property ItemType As String
    Property Quantity As Integer
    ReadOnly Property Stacks As Boolean
    ReadOnly Property Name As String
    ReadOnly Property CanHeal As Boolean
    ReadOnly Property CanEquip As Boolean
    ReadOnly Property IsWeapon As Boolean
    ReadOnly Property IsArmor As Boolean
    ReadOnly Property IsBroken As Boolean
    Sub DoWear(wear As Integer)
    ReadOnly Property Durability As Integer
    ReadOnly Property MaximumDurability As Integer
    ReadOnly Property MaximumAttack As Integer
    ReadOnly Property MaximumDefend As Integer
    ReadOnly Property Attack As Integer
    ReadOnly Property Defend As Integer
End Interface
