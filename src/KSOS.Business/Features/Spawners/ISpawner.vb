Public Interface ISpawner
    Sub SetSpawnWeight(characterType As String, weight As Integer)
    Sub DoSpawn(character As ICharacter)
End Interface
