Friend Class Loot
    Inherits LootDataClient
    Implements ILoot
    Private ReadOnly FeatureId As Integer
    Public Sub New(data As WorldData, featureId As Integer)
        MyBase.New(data, featureId)
        Me.FeatureId = featureId
    End Sub
    Public Sub SetLoot(itemType As ItemType, quantity As Integer) Implements ILoot.SetLoot
        LootData.ItemType = itemType
        LootData.Quantity = quantity
    End Sub

    Public Sub DoLoot(character As ICharacter) Implements ILoot.DoLoot
        Dim world As New World(WorldData)
        Dim item = world.CreateItem(LootData.ItemType, LootData.Quantity)
        character.AddMessage($"{character.Name} finds {item.Name}(x{item.Quantity})")
        character.AddItem(item)
        character.Location.RemoveFeature(New Feature(WorldData, FeatureId))
    End Sub
End Class
