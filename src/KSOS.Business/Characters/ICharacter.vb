﻿Public Interface ICharacter
    Inherits IThingie
    Property Location As ILocation
    Property Name As String
    Sub Move(direction As String)
    Function Run() As Boolean
    Sub MakeAttack(defender As ICharacter, index As Integer)
    Function RollDefend() As Integer
    Function RollAttack() As Integer
    Sub TakeDamage(damage As Integer)
    Sub Kill()
    Sub AddItem(item As IItem)
    Sub RemoveItem(item As IItem)
    Function HasItemQuantity(itemType As String, quantity As Integer) As Boolean
    Sub RemoveItemQuantity(itemType As String, quantity As Integer)
    Sub AddItemQuantity(itemType As String, quantity As Integer)
    Property Health As Integer
    ReadOnly Property MaximumHealth As Integer
    ReadOnly Property CharacterType As String
    ReadOnly Property CanMove As Boolean
    ReadOnly Property CanRun As Boolean
    ReadOnly Property CanFight As Boolean
    ReadOnly Property IsDead As Boolean
    Property XP As Integer
    ReadOnly Property KillVerb As String
    ReadOnly Property World As IWorld
    ReadOnly Property CanPickUpItems As Boolean
    ReadOnly Property CanInteract As Boolean
    ReadOnly Property HasItems As Boolean
    ReadOnly Property Items As IEnumerable(Of IItem)
    ReadOnly Property HasMessages As Boolean
    ReadOnly Property NextMessage As IMessage
    ReadOnly Property HasAnyEquipment As Boolean
    Sub AddMessage(ParamArray lines As String())
    Sub DismissMessage()
    Function GetStatistic(statisticType As String) As Integer
    Sub Train(trainingStatistic As ITrainingStatistic)
    Sub Equip(item As IItem)
    Function HasEquipment(equipSlot As String) As Boolean
    Sub Unequip(equipSlot As String)
    Function Equipment(equipSlot As String) As IItem
    ReadOnly Property EquippedSlots As IEnumerable(Of String)
    ReadOnly Property EquippedItems As IEnumerable(Of IItem)
    Sub WearWeapon(wear As Integer)
    Sub WearArmor(wear As Integer)
    Function HasItem(item As IItem) As Boolean
    ReadOnly Property Attack As Integer
    ReadOnly Property MaximumAttack As Integer
    ReadOnly Property Defend As Integer
    ReadOnly Property MaximumDefend As Integer
End Interface
