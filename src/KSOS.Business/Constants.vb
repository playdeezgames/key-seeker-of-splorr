﻿Imports System.Runtime.CompilerServices

Public Module Constants
    Private ReadOnly statisticNames As IReadOnlyDictionary(Of String, String) =
        New Dictionary(Of String, String) From
        {
            {StatisticType.MaximumHealth, "Maximum Health"}
        }
    <Extension>
    Public Function StatisticTypeName(statisticType As String) As String
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
            {LocationType.Tower, "a tower"},
            {LocationType.Hut, "a hut"},
            {LocationType.Ufo, "a UFO"},
            {LocationType.Machine, "a machine"}
        }
    <Extension>
    Public Function LocationTypeName(locationType As String) As String
        Return locationTypeNames(locationType)
    End Function
    Private ReadOnly directionNames As IReadOnlyDictionary(Of String, String) =
        New Dictionary(Of String, String) From
        {
            {Direction.North, "North"},
            {Direction.East, "East"},
            {Direction.South, "South"},
            {Direction.West, "West"},
            {Direction.Inside, "In"},
            {Direction.Outside, "Out"},
            {Direction.Up, "Up"},
            {Direction.Down, "Down"},
            {Direction.Portal, "into a Portal"}
        }
    <Extension>
    Public Function DirectionName(direction As String) As String
        Return directionNames(direction)
    End Function
    Private ReadOnly routeTypeNames As IReadOnlyDictionary(Of String, String) =
        New Dictionary(Of String, String) From
        {
            {RouteType.ForestPath, "Forest Path"},
            {RouteType.DirtRoad, "Dirt Road"},
            {RouteType.TownGate, "Town Gate"},
            {RouteType.GraveyardPath, "Graveyard Path"},
            {RouteType.GraveyardGate, "Graveyard Gate"},
            {RouteType.RuinsEntrance, "Ruins Entrance"},
            {RouteType.RuinsPath, "Ruins Path"},
            {RouteType.Door, "Door"},
            {RouteType.Stairs, "Stairs"},
            {RouteType.Tunnel, "Tunnel"},
            {RouteType.TowerEntrance, "Tower Entrance"},
            {RouteType.Portal, "Shimmering Oval"},
            {RouteType.Hallway, "Metallic Hallway"},
            {RouteType.UfoRamp, "Otherworldly Ramp"},
            {RouteType.Hatch, "Circular Hatch"}
        }
    <Extension>
    Public Function RouteTypeName(routeType As String) As String
        Return routeTypeNames(routeType)
    End Function
End Module
