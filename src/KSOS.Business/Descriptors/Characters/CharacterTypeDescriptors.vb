﻿Imports System.Runtime.CompilerServices

Friend Module CharacterTypeDescriptors
    Friend ReadOnly Property AllCharacterTypes As IReadOnlyList(Of String)
        Get
            Return characterTypeDescriptors.Keys.ToList
        End Get
    End Property
    '{
    '    CharacterType.N00b,
    '    New CharacterTypeDescriptor With
    '    {
    '        .Name = "N00b",
    '        .SpawnCount = 1,
    '        .SpawnLocations = New List(Of LocationType) From {LocationType.Town},
    '        .KillVerb = "kills",
    '        .Provision = AddressOf ProvisionN00b,
    '        .Statistics = New Dictionary(Of StatisticType, Integer) From
    '        {
    '            {StatisticType.MaximumHealth, 3},
    '            {StatisticType.Wounds, 0},
    '            {StatisticType.MaximumAttack, 1},
    '            {StatisticType.Attack, 3},
    '            {StatisticType.MaximumDefend, 2},
    '            {StatisticType.Defend, 4},
    '            {StatisticType.XP, 0}
    '        }
    '    }
    '},
    Private ReadOnly characterTypeDescriptors As IReadOnlyDictionary(Of String, CharacterTypeDescriptor) =
        New Dictionary(Of String, CharacterTypeDescriptor) From
        {
            {
                CharacterType.N00b,
                New CharacterTypeDescriptor With
                {
                    .Name = "N00b",
                    .SpawnCount = 1,
                    .SpawnLocations = New List(Of String) From {LocationType.Town},
                    .KillVerb = "kills",
                    .Provision = AddressOf ProvisionN00b,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 3},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 1},
                        {StatisticType.Attack, 3},
                        {StatisticType.MaximumDefend, 2},
                        {StatisticType.Defend, 4},
                        {StatisticType.XP, 0}
                    }
                }
            },
            {
                CharacterType.Skeleton,
                New CharacterTypeDescriptor With
                {
                    .Name = "Skeleton",
                    .SpawnCount = 50,
                    .KillVerb = "kills",
                    .SpawnLocations = New List(Of String) From {LocationType.Graveyard},
                    .Provision = AddressOf ProvisionSkeleton,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 1},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 2},
                        {StatisticType.Attack, 4},
                        {StatisticType.MaximumDefend, 2},
                        {StatisticType.Defend, 2},
                        {StatisticType.XP, 1}
                    }
                }
            },
            {
                CharacterType.Zombie,
                New CharacterTypeDescriptor With
                {
                    .Name = "Zombie",
                    .SpawnCount = 25,
                    .KillVerb = "consumes the brains of",
                    .SpawnLocations = New List(Of String) From {LocationType.Graveyard},
                    .Provision = AddressOf ProvisionZombie,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 1},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 2},
                        {StatisticType.Attack, 4},
                        {StatisticType.MaximumDefend, 3},
                        {StatisticType.Defend, 3},
                        {StatisticType.XP, 1}
                    }
                }
            },
            {
                CharacterType.Blob,
                New CharacterTypeDescriptor With
                {
                    .Name = "Blob",
                    .SpawnCount = 300,
                    .KillVerb = "slimes",
                    .SpawnLocations = New List(Of String) From {LocationType.Forest},
                    .Provision = AddressOf ProvisionBlob,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 1},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 1},
                        {StatisticType.Attack, 2},
                        {StatisticType.MaximumDefend, 1},
                        {StatisticType.Defend, 1},
                        {StatisticType.XP, 1}
                    }
                }
            },
            {
                CharacterType.CellarRat,
                New CharacterTypeDescriptor With
                {
                    .Name = "Cellar Rat",
                    .SpawnCount = 5,
                    .KillVerb = "gnaws the flesh of",
                    .SpawnLocations = New List(Of String) From {LocationType.Cellar},
                    .Provision = AddressOf ProvisionCellarRat,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 1},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 1},
                        {StatisticType.Attack, 2},
                        {StatisticType.MaximumDefend, 1},
                        {StatisticType.Defend, 1},
                        {StatisticType.XP, 1}
                    }
                }
            },
            {
                CharacterType.SewerRat,
                New CharacterTypeDescriptor With
                {
                    .Name = "Sewer Rat",
                    .SpawnCount = 75,
                    .KillVerb = "feasts on the innard of",
                    .SpawnLocations = New List(Of String) From {LocationType.Sewer},
                    .Provision = AddressOf ProvisionSewerRat,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 1},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 1},
                        {StatisticType.Attack, 4},
                        {StatisticType.MaximumDefend, 1},
                        {StatisticType.Defend, 1},
                        {StatisticType.XP, 1}
                    }
                }
            },
            {
                CharacterType.KingRat,
                New CharacterTypeDescriptor With
                {
                    .Name = "King Rat",
                    .SpawnCount = 1,
                    .KillVerb = "cavorts in the intestines of",
                    .SpawnLocations = New List(Of String) From {LocationType.Sewer},
                    .Provision = AddressOf ProvisionKingRat,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 2},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 2},
                        {StatisticType.Attack, 4},
                        {StatisticType.MaximumDefend, 2},
                        {StatisticType.Defend, 2},
                        {StatisticType.XP, 2}
                    }
                }
            }
        }
    <Extension>
    Friend Function CharacterTypeDescriptor(characterType As String) As CharacterTypeDescriptor
        Return characterTypeDescriptors(characterType)
    End Function
End Module
