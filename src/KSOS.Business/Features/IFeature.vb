Public Interface IFeature
    Inherits IThingie
    ReadOnly Property Name As String
    ReadOnly Property FeatureType As String
    ReadOnly Property Shoppe As IShoppe
    ReadOnly Property Training As ITraining
    ReadOnly Property Spawner As ISpawner
    ReadOnly Property Loot As ILoot
End Interface
