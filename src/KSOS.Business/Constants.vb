Imports System.Runtime.CompilerServices

Public Module Constants
    Private ReadOnly statisticNames As IReadOnlyDictionary(Of StatisticType, String) =
        New Dictionary(Of StatisticType, String) From
        {
            {StatisticType.MaximumHealth, "Maximum Health"}
        }
    <Extension>
    Public Function Name(statisticType As StatisticType) As String
        Return statisticNames(statisticType)
    End Function
    Private ReadOnly equipSlotNames As IReadOnlyDictionary(Of EquipSlot, String) =
        New Dictionary(Of EquipSlot, String) From
        {
            {EquipSlot.Weapon, "Weapon"},
            {EquipSlot.Shield, "Shield"},
            {EquipSlot.Torso, "Torso"},
            {EquipSlot.Head, "Head"}
        }
    <Extension>
    Public Function Name(equipSlot As EquipSlot) As String
        Return equipSlotNames(equipSlot)
    End Function
End Module
