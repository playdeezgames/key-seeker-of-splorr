Friend Module CharacterInitializer
    Friend Sub Initialize(world As IWorld)
        For Each characterType In AllCharacterTypes
            Dim descriptor = characterType.CharacterTypeDescriptor
            Dim spawnCount = descriptor.SpawnCount
            Dim spawnLocations = world.Locations.Where(Function(x) descriptor.SpawnLocations.Contains(x.LocationType))
            While spawnCount > 0
                Dim spawnLocation = RNG.FromEnumerable(spawnLocations)
                world.CreateCharacter(characterType, spawnLocation)
                spawnCount -= 1
            End While
        Next
    End Sub
End Module
