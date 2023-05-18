Imports System.Runtime.CompilerServices

Friend Module Descriptors
    Friend ReadOnly Property AllCharacterTypes As IReadOnlyList(Of CharacterType)
        Get
            Return characterTypeDescriptors.Keys.ToList
        End Get
    End Property
    Private ReadOnly characterTypeDescriptors As IReadOnlyDictionary(Of CharacterType, CharacterTypeDescriptor) =
        New Dictionary(Of CharacterType, CharacterTypeDescriptor) From
        {
            {
                CharacterType.N00b,
                New CharacterTypeDescriptor With
                {
                    .Name = "N00b",
                    .SpawnCount = 0,
                    .KillVerb = "kills",
                    .Provision = AddressOf ProvisionN00b,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
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
                CharacterType.Blob,
                New CharacterTypeDescriptor With
                {
                    .Name = "Blob",
                    .SpawnCount = 200,
                    .KillVerb = "slimes",
                    .SpawnLocations = New List(Of LocationType) From {LocationType.Forest},
                    .Provision = AddressOf ProvisionBlob,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
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
                    .SpawnCount = 10,
                    .KillVerb = "gnaws the flesh of",
                    .SpawnLocations = New List(Of LocationType) From {LocationType.Cellar},
                    .Provision = AddressOf ProvisionCellarRat,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
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
                    .SpawnLocations = New List(Of LocationType) From {LocationType.Sewer},
                    .Provision = AddressOf ProvisionSewerRat,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
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
                    .SpawnLocations = New List(Of LocationType) From {LocationType.Sewer},
                    .Provision = AddressOf ProvisionKingRat,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
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
    Friend Function Descriptor(characterType As CharacterType) As CharacterTypeDescriptor
        Return characterTypeDescriptors(characterType)
    End Function
    Private ReadOnly itemTypeDescriptors As IReadOnlyDictionary(Of ItemType, ItemTypeDescriptor) =
        New Dictionary(Of ItemType, ItemTypeDescriptor) From
        {
            {
                ItemType.Chikkin,
                New ItemTypeDescriptor With
                {
                    .Name = "Chikkin",
                    .Stacks = True,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
                    {
                        {StatisticType.Healing, 1}
                    }
                }
            },
            {
                ItemType.RatTail,
                New ItemTypeDescriptor With
                {
                    .Name = "Rat Tail",
                    .Stacks = True,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.GraveyardKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Graveyard Key",
                    .Stacks = False,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.MachineKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Machine Key",
                    .Stacks = False,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.SewerKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Sewer Key",
                    .Stacks = False,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.TowerKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Tower Key",
                    .Stacks = False,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.UfoKey,
                New ItemTypeDescriptor With
                {
                    .Name = "UFO Key",
                    .Stacks = False,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.Jools,
                New ItemTypeDescriptor With
                {
                    .Name = "Jools",
                    .Stacks = True,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            }
        }
    <Extension>
    Friend Function Descriptor(ItemType As ItemType) As ItemTypeDescriptor
        Return itemTypeDescriptors(ItemType)
    End Function
End Module
