Imports System.Runtime.CompilerServices

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
                    .KillVerb = "feasts on the innards of",
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
            },
            {
                CharacterType.Goblin,
                New CharacterTypeDescriptor With
                {
                    .Name = "Goblin",
                    .SpawnCount = 75,
                    .KillVerb = "dances on the corpse of",
                    .SpawnLocations = New List(Of String) From {LocationType.Tower},
                    .Provision = AddressOf ProvisionGoblin,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 1},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 2},
                        {StatisticType.Attack, 4},
                        {StatisticType.MaximumDefend, 1},
                        {StatisticType.Defend, 1},
                        {StatisticType.XP, 1}
                    }
                }
            },
            {
                CharacterType.Orc,
                New CharacterTypeDescriptor With
                {
                    .Name = "Orc",
                    .SpawnCount = 50,
                    .KillVerb = "harvests the heart of",
                    .SpawnLocations = New List(Of String) From {LocationType.Tower},
                    .Provision = AddressOf ProvisionOrc,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 1},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 3},
                        {StatisticType.Attack, 6},
                        {StatisticType.MaximumDefend, 2},
                        {StatisticType.Defend, 2},
                        {StatisticType.XP, 1}
                    }
                }
            },
            {
                CharacterType.Cyclops,
                New CharacterTypeDescriptor With
                {
                    .Name = "Cyclops",
                    .SpawnCount = 25,
                    .KillVerb = "plucks out one eye of",
                    .SpawnLocations = New List(Of String) From {LocationType.Tower},
                    .Provision = AddressOf ProvisionCyclops,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 2},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 3},
                        {StatisticType.Attack, 6},
                        {StatisticType.MaximumDefend, 3},
                        {StatisticType.Defend, 3},
                        {StatisticType.XP, 2}
                    }
                }
            },
            {
                CharacterType.MoonPerson,
                New CharacterTypeDescriptor With
                {
                    .Name = "Moon Person",
                    .SpawnCount = 20,
                    .KillVerb = "does lewd experiments on",
                    .SpawnLocations = New List(Of String) From {LocationType.Ufo},
                    .Provision = AddressOf ProvisionMoonPerson,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 3},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 4},
                        {StatisticType.Attack, 8},
                        {StatisticType.MaximumDefend, 4},
                        {StatisticType.Defend, 4},
                        {StatisticType.XP, 3}
                    }
                }
            },
            {
                CharacterType.Mummy,
                New CharacterTypeDescriptor With
                {
                    .Name = "Mummy",
                    .SpawnCount = 30,
                    .KillVerb = "mummifies",
                    .SpawnLocations = New List(Of String) From {LocationType.Ruins},
                    .Provision = AddressOf ProvisionMummy,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 2},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 3},
                        {StatisticType.Attack, 6},
                        {StatisticType.MaximumDefend, 4},
                        {StatisticType.Defend, 4},
                        {StatisticType.XP, 2}
                    }
                }
            },
            {
                CharacterType.Gargoyle,
                New CharacterTypeDescriptor With
                {
                    .Name = "Gargoyle",
                    .SpawnCount = 15,
                    .KillVerb = "paints itself in the blood of",
                    .SpawnLocations = New List(Of String) From {LocationType.Ruins},
                    .Provision = AddressOf ProvisionGargoyle,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 3},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 4},
                        {StatisticType.Attack, 8},
                        {StatisticType.MaximumDefend, 5},
                        {StatisticType.Defend, 5},
                        {StatisticType.XP, 3}
                    }
                }
            },
            {
                CharacterType.Guardian,
                New CharacterTypeDescriptor With
                {
                    .Name = "Machine Guardian",
                    .SpawnCount = 15,
                    .KillVerb = "makes a greasy spot where once stood",
                    .SpawnLocations = New List(Of String) From {LocationType.Ruins},
                    .Provision = AddressOf ProvisionGuardian,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.MaximumHealth, 10},
                        {StatisticType.Wounds, 0},
                        {StatisticType.MaximumAttack, 10},
                        {StatisticType.Attack, 20},
                        {StatisticType.MaximumDefend, 10},
                        {StatisticType.Defend, 10},
                        {StatisticType.XP, 10}
                    }
                }
            }
        }
    <Extension>
    Friend Function CharacterTypeDescriptor(characterType As String) As CharacterTypeDescriptor
        Return characterTypeDescriptors(characterType)
    End Function
End Module
