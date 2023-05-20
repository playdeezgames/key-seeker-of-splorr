Friend Class Spawner
    Inherits SpawnerDataClient
    Implements ISpawner
    ReadOnly Property FeatureId As Integer
    Public Sub New(data As WorldData, featureId As Integer)
        MyBase.New(data, featureId)
        Me.FeatureId = featureId
    End Sub
    Public Sub SetSpawnWeight(characterType As CharacterType, weight As Integer) Implements ISpawner.SetSpawnWeight
        SpawnerData.Generator(characterType) = weight
    End Sub
    Public Sub DoSpawn(character As ICharacter) Implements ISpawner.DoSpawn
        Dim world = New World(WorldData)
        Dim enemy = world.CreateCharacter(RNG.FromGenerator(SpawnerData.Generator), character.Location)
        character.AddMessage($"{enemy.Name} suddenly appears!")
        character.Location.RemoveFeature(New Feature(WorldData, FeatureId))
    End Sub
End Class
