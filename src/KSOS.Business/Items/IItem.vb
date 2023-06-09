﻿Public Interface IItem
    Inherits IThingie
    ReadOnly Property ItemType As String
    Property Quantity As Integer
    ReadOnly Property Stacks As Boolean
    ReadOnly Property Name As String
    ReadOnly Property CanEquip As Boolean
    ReadOnly Property IsWeapon As Boolean
    ReadOnly Property IsArmor As Boolean
    ReadOnly Property IsBroken As Boolean
    Sub DoWear(wear As Integer)
    Function CanUse(character As ICharacter) As Boolean
    Sub Use(character As ICharacter)
    ReadOnly Property Durability As Integer
    ReadOnly Property MaximumDurability As Integer
    ReadOnly Property MaximumAttack As Integer
    ReadOnly Property MaximumDefend As Integer
    ReadOnly Property Attack As Integer
    ReadOnly Property Defend As Integer
    ReadOnly Property UseVerb As String
End Interface
