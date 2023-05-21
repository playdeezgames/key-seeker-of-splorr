Friend Module GraveyardInitializer
    Const Columns = 7
    Const Rows = 7
    Friend Sub Initialize(world As IWorld)
        Dim locations(Columns - 1, Rows - 1) As ILocation
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                locations(column, row) = world.CreateLocation(LocationType.Graveyard)
            Next
        Next
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                If row > 0 Then
                    locations(column, row).CreateRoute(Direction.North, RouteType.GraveyardPath, locations(column, row - 1))
                End If
                If column > 0 Then
                    locations(column, row).CreateRoute(Direction.West, RouteType.GraveyardPath, locations(column - 1, row))
                End If
                If row < Rows - 1 Then
                    locations(column, row).CreateRoute(Direction.South, RouteType.GraveyardPath, locations(column, row + 1))
                End If
                If column < Columns - 1 Then
                    locations(column, row).CreateRoute(Direction.East, RouteType.GraveyardPath, locations(column + 1, row))
                End If
            Next
        Next
        Dim forestLocation = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.ForestCorner AndAlso Not x.HasRoute(Direction.Inside)))
        Dim graveyardEntrance = forestLocation.CreateRoute(Direction.Inside, RouteType.GraveyardGate, locations(Columns \ 2, Rows - 1))
        graveyardEntrance.RequiredItemType = ItemType.GraveyardKey
        locations(Columns \ 2, Rows - 1).CreateRoute(Direction.South, RouteType.GraveyardGate, forestLocation)
        InitializeGraves(world)
    End Sub
    ReadOnly graveTypeGenerator As IReadOnlyDictionary(Of String, Integer) =
        New Dictionary(Of String, Integer) From
        {
            {FeatureType.Spawner, 1},
            {FeatureType.Loot, 1}
        }
    Private Sub InitializeGraves(world As IWorld)
        Dim graveLocations = world.Locations.Where(Function(x) x.LocationType = LocationType.Graveyard)
        Dim keyLocation = RNG.FromEnumerable(graveLocations)
        For Each location In graveLocations
            Dim grave As IFeature
            If location Is keyLocation Then
                grave = world.CreateFeature("grave", FeatureType.Loot)
                grave.Loot.SetLoot(ItemType.TowerKey, 1)
            Else
                Select Case RNG.FromGenerator(graveTypeGenerator)
                    Case FeatureType.Spawner
                        grave = world.CreateFeature("grave", FeatureType.Spawner)
                        grave.Spawner.SetSpawnWeight(CharacterType.Skeleton, 2)
                        grave.Spawner.SetSpawnWeight(CharacterType.Zombie, 1)
                    Case FeatureType.Loot
                        grave = world.CreateFeature("grave", FeatureType.Loot)
                        grave.Loot.SetLoot(ItemType.Jools, RNG.RollDice("2d6"))
                    Case Else
                        Throw New NotImplementedException
                End Select
            End If
            location.AddFeature(grave)
        Next
    End Sub
End Module
