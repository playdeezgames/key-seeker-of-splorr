Imports System.Runtime.CompilerServices

Friend Module ItemTypeDescriptors
    Private ReadOnly itemTypeDescriptors As IReadOnlyDictionary(Of String, ItemTypeDescriptor) =
        New Dictionary(Of String, ItemTypeDescriptor) From
        {
            {
                ItemType.Chikkin,
                New ItemTypeDescriptor With
                {
                    .Name = "Chikkin",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer),
                    .CanUse = Function(character)
                                  Return character.Health < character.MaximumHealth
                              End Function,
                    .Use = Sub(character)
                               character.Health += 1
                               character.AddMessage($"{character.Name} eats the chikkin, and now has {character.Health}/{character.MaximumHealth} health.")
                           End Sub,
                    .UseVerb = "Nom"
                }
            },
            {
                ItemType.Potion,
                New ItemTypeDescriptor With
                {
                    .Name = "Potion",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer),
                    .CanUse = Function(character)
                                  Return character.Health < character.MaximumHealth
                              End Function,
                    .Use = Sub(character)
                               character.Health += 5
                               character.AddMessage($"{character.Name} drinks the potion, and now has {character.Health}/{character.MaximumHealth} health.")
                           End Sub,
                    .UseVerb = "Drink"
                }
            },
            {
                ItemType.TownPortal,
                New ItemTypeDescriptor With
                {
                    .Name = "Scroll of Town Portal",
                    .Stacks = False,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer),
                    .CanUse = Function(character)
                                  Return character.Location.LocationType <> LocationType.Town
                              End Function,
                    .Use = Sub(character)
                               Dim townLocation = character.World.Locations.First(Function(x) x.LocationType = LocationType.Town)
                               Dim route = character.Location.CreateRoute(Direction.Portal, RouteType.Portal, townLocation)
                               route.SingleUse = True
                               route = townLocation.CreateRoute(Direction.Portal, RouteType.Portal, character.Location)
                               route.SingleUse = True
                               character.AddMessage($"{character.Name} reads the scroll, and a portal appears!")
                           End Sub,
                    .UseVerb = "Read"
                }
            },
            {
                ItemType.HolyWater,
                New ItemTypeDescriptor With
                {
                    .Name = "Holy Water",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                    },
                    .CanUse = Function(character)
                                  Return character.CanFight
                              End Function,
                    .Use = Sub(character)
                               character.AddMessage($"{character.Name} moistens his enemy!")
                           End Sub,
                    .UseVerb = "Splash"
                }
            },
            {
                ItemType.GoblinEar,
                New ItemTypeDescriptor With
                {
                    .Name = "Goblin Ear",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.OrcTooth,
                New ItemTypeDescriptor With
                {
                    .Name = "Orc Tooth",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.CyclopsEyeball,
                New ItemTypeDescriptor With
                {
                    .Name = "Cyclops Eyeball",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.SkullFragment,
                New ItemTypeDescriptor With
                {
                    .Name = "Skull Fragment",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.ZombieTaint,
                New ItemTypeDescriptor With
                {
                    .Name = "Zombie Taint",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.Feather,
                New ItemTypeDescriptor With
                {
                    .Name = "Feather",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.Dagger,
                New ItemTypeDescriptor With
                {
                    .Name = "Dagger",
                    .Stacks = False,
                    .EquipSlot = EquipSlot.Weapon,
                    .Statistics = New Dictionary(Of String, Integer) From
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
                    .Statistics = New Dictionary(Of String, Integer) From
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
                    .Statistics = New Dictionary(Of String, Integer) From
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
                    .Statistics = New Dictionary(Of String, Integer) From
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
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.Attack, 13},
                        {StatisticType.MaximumAttack, 4},
                        {StatisticType.Durability, 100},
                        {StatisticType.Wear, 0}
                    }
                }
            },
            {
                ItemType.Shield,
                New ItemTypeDescriptor With
                {
                    .Name = "Shield",
                    .Stacks = False,
                    .EquipSlot = EquipSlot.Shield,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.Defend, 2},
                        {StatisticType.MaximumDefend, 1},
                        {StatisticType.Durability, 50},
                        {StatisticType.Wear, 0}
                    }
                }
            },
            {
                ItemType.Helmet,
                New ItemTypeDescriptor With
                {
                    .Name = "Helmet",
                    .Stacks = False,
                    .EquipSlot = EquipSlot.Head,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.Defend, 1},
                        {StatisticType.MaximumDefend, 0},
                        {StatisticType.Durability, 25},
                        {StatisticType.Wear, 0}
                    }
                }
            },
            {
                ItemType.LeatherArmor,
                New ItemTypeDescriptor With
                {
                    .Name = "Leather Armor",
                    .Stacks = False,
                    .EquipSlot = EquipSlot.Torso,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.Defend, 2},
                        {StatisticType.MaximumDefend, 1},
                        {StatisticType.Durability, 25},
                        {StatisticType.Wear, 0}
                    }
                }
            },
            {
                ItemType.ChainMail,
                New ItemTypeDescriptor With
                {
                    .Name = "Chain Mail",
                    .Stacks = False,
                    .EquipSlot = EquipSlot.Torso,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.Defend, 2},
                        {StatisticType.MaximumDefend, 1},
                        {StatisticType.Durability, 100},
                        {StatisticType.Wear, 0}
                    }
                }
            },
            {
                ItemType.PlateMail,
                New ItemTypeDescriptor With
                {
                    .Name = "Plate Mail",
                    .Stacks = False,
                    .EquipSlot = EquipSlot.Torso,
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticType.Defend, 4},
                        {StatisticType.MaximumDefend, 2},
                        {StatisticType.Durability, 150},
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
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.SewerRatTail,
                New ItemTypeDescriptor With
                {
                    .Name = "Sewer Rat Tail",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.GraveyardKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Graveyard Key",
                    .Stacks = False,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.MachineKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Machine Key",
                    .Stacks = False,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.SewerKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Sewer Key",
                    .Stacks = False,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.TowerKey,
                New ItemTypeDescriptor With
                {
                    .Name = "Tower Key",
                    .Stacks = False,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.UfoKey,
                New ItemTypeDescriptor With
                {
                    .Name = "UFO Key",
                    .Stacks = False,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            },
            {
                ItemType.Jools,
                New ItemTypeDescriptor With
                {
                    .Name = "Jools",
                    .Stacks = True,
                    .EquipSlot = Nothing,
                    .Statistics = New Dictionary(Of String, Integer)
                }
            }
        }
    <Extension>
    Friend Function ItemTypeDescriptor(ItemType As String) As ItemTypeDescriptor
        Return itemTypeDescriptors(ItemType)
    End Function
End Module
