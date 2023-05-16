Imports System.Runtime.CompilerServices

Friend Module Descriptors
    Private ReadOnly characterTypeDescriptors As IReadOnlyDictionary(Of CharacterType, CharacterTypeDescriptor) =
        New Dictionary(Of CharacterType, CharacterTypeDescriptor) From
        {
            {
                CharacterType.N00b,
                New CharacterTypeDescriptor With
                {
                    .Name = "N00b",
                    .SpawnCount = 0,
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
                    .SpawnCount = 100,
                    .SpawnLocations = New List(Of LocationType) From {LocationType.Forest},
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
                ItemType.GraveyardKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Graveyard Key"
                }
            },
            {
                ItemType.MachineKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Machine Key"
                }
            },
            {
                ItemType.SewerKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Sewer Key"
                }
            },
            {
                ItemType.TowerKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Tower Key"
                }
            },
            {
                ItemType.UfoKey,
                New ItemTypeDescriptor With
                {
                    .Name = "UFO Key"
                }
            }
        }
    <Extension>
    Friend Function Descriptor(ItemType As ItemType) As ItemTypeDescriptor
        Return itemTypeDescriptors(ItemType)
    End Function
End Module
