Imports System.Runtime.CompilerServices

Public Module Constants
    Private ReadOnly statisticNames As IReadOnlyDictionary(Of StatisticType, String) =
        New Dictionary(Of StatisticType, String) From
        {
            {StatisticType.MaximumHealth, "Maximum Health"}
        }
    <Extension>
    Public Function StatisticTypeName(statisticType As StatisticType) As String
        Return statisticNames(statisticType)
    End Function
    Private ReadOnly equipSlotNames As IReadOnlyDictionary(Of String, String) =
        New Dictionary(Of String, String) From
        {
            {EquipSlot.Weapon, "Weapon"},
            {EquipSlot.Shield, "Shield"},
            {EquipSlot.Torso, "Torso"},
            {EquipSlot.Head, "Head"}
        }
    <Extension>
    Public Function EquipSlotName(equipSlot As String) As String
        Return equipSlotNames(equipSlot)
    End Function
    Private ReadOnly locationTypeNames As IReadOnlyDictionary(Of String, String) =
        New Dictionary(Of String, String) From
        {
            {LocationType.ForestCorner, "a forest"},
            {LocationType.Forest, "a forest"},
            {LocationType.ForestCenter, "a forest"},
            {LocationType.Town, "town"},
            {LocationType.TownEdge, "town"},
            {LocationType.Graveyard, "a graveyard"},
            {LocationType.Ruins, "a ruins"},
            {LocationType.Inn, "an inn"},
            {LocationType.Cellar, "a cellar"},
            {LocationType.Sewer, "a sewer"},
            {LocationType.Knackery, "a knackery"},
            {LocationType.Blacksmith, "a forge"},
            {LocationType.Tower, "a tower"}
        }
    <Extension>
    Public Function LocationTypeName(locationType As String) As String
        Return locationTypeNames(locationType)
    End Function
End Module
