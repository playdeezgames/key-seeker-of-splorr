Public Interface ISpawner
    Sub SetSpawnWeight(characterType As CharacterType, weight As Integer)
    Sub DoSpawn(character As ICharacter)
End Interface
