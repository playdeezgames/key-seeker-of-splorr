Imports System.Runtime.CompilerServices

Friend Module ItemTypeDescriptors
    Private ReadOnly itemTypeDescriptors As IReadOnlyDictionary(Of ItemType, ItemTypeDescriptor) =
        New Dictionary(Of ItemType, ItemTypeDescriptor) From
        {
            {
                ItemType.Chikkin,
                New ItemTypeDescriptor With
                {
                    .Name = "Chikkin",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
                    {
                        {StatisticType.Healing, 1}
                    }
                }
            },
            {
                ItemType.Dagger,
                New ItemTypeDescriptor With
                {
                    .Name = "Dagger",
                    .Stacks = False,
                    .EquipSlot = EquipSlot.Weapon,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
                    {
                        {StatisticType.Attack, 1},
                        {StatisticType.MaximumAttack, 0},
                        {StatisticType.Durability, 20},
                        {StatisticType.Wear, 0}
                    }
                }
            },
            {
                ItemType.Shortsword,
                New ItemTypeDescriptor With
                {
                    .Name = "Shortsword",
                    .Stacks = False,
                    .EquipSlot = EquipSlot.Weapon,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
                    {
                        {StatisticType.Attack, 4},
                        {StatisticType.MaximumAttack, 1},
                        {StatisticType.Durability, 40},
                        {StatisticType.Wear, 0}
                    }
                }
            },
            {
                ItemType.Broadsword,
                New ItemTypeDescriptor With
                {
                    .Name = "Broadsword",
                    .Stacks = False,
                    .EquipSlot = EquipSlot.Weapon,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
                    {
                        {StatisticType.Attack, 7},
                        {StatisticType.MaximumAttack, 2},
                        {StatisticType.Durability, 60},
                        {StatisticType.Wear, 0}
                    }
                }
            },
            {
                ItemType.Longsword,
                New ItemTypeDescriptor With
                {
                    .Name = "Longsword",
                    .Stacks = False,
                    .EquipSlot = EquipSlot.Weapon,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
                    {
                        {StatisticType.Attack, 10},
                        {StatisticType.MaximumAttack, 3},
                        {StatisticType.Durability, 80},
                        {StatisticType.Wear, 0}
                    }
                }
            },
            {
                ItemType.Axe,
                New ItemTypeDescriptor With
                {
                    .Name = "Axe",
                    .Stacks = False,
                    .EquipSlot = EquipSlot.Weapon,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
                    {
                        {StatisticType.Attack, 13},
                        {StatisticType.MaximumAttack, 4},
                        {StatisticType.Durability, 100},
                        {StatisticType.Wear, 0}
                    }
                }
            },
            {
                ItemType.CellarRatTail,
                New ItemTypeDescriptor With
                {
                    .Name = "Cellar Rat Tail",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.SewerRatTail,
                New ItemTypeDescriptor With
                {
                    .Name = "Sewer Rat Tail",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.GraveyardKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Graveyard Key",
                    .Stacks = False,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.MachineKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Machine Key",
                    .Stacks = False,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.SewerKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Sewer Key",
                    .Stacks = False,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.TowerKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Tower Key",
                    .Stacks = False,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.UfoKey,
                New ItemTypeDescriptor With
                {
                    .Name = "UFO Key",
                    .Stacks = False,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            },
            {
                ItemType.Jools,
                New ItemTypeDescriptor With
                {
                    .Name = "Jools",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of StatisticType, Integer)
                }
            }
        }
    <Extension>
    Friend Function Descriptor(ItemType As ItemType) As ItemTypeDescriptor
        Return itemTypeDescriptors(ItemType)
    End Function
End Module
