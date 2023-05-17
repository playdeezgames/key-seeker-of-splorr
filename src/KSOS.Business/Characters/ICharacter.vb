Public Interface ICharacter
    Inherits IThingie
    Property Location As ILocation
    Property Name As String
    Sub Move(direction As Direction)
    Function Run() As Boolean
    Function MakeAttack(defender As ICharacter, index As Integer) As IEnumerable(Of String)
    Function RollDefend() As Integer
    Function RollAttack() As Integer
    Sub TakeDamage(damage As Integer)
    Sub Kill()
    Sub AddItem(item As IItem)
    Sub RemoveItem(item As IItem)
    Function HasItemQuantity(itemType As ItemType, quantity As Integer) As Boolean
    Sub RemoveItemQuantity(itemType As ItemType, quantity As Integer)
    Sub AddItemQuantity(itemType As ItemType, quantity As Integer)
    ReadOnly Property Health As Integer
    ReadOnly Property MaximumHealth As Integer
    ReadOnly Property CharacterType As CharacterType
    ReadOnly Property CanMove As Boolean
    ReadOnly Property CanRun As Boolean
    ReadOnly Property CanFight As Boolean
    ReadOnly Property IsDead As Boolean
    Property XP As Integer
    ReadOnly Property KillVerb As String
    ReadOnly Property World As IWorld
    ReadOnly Property CanPickUpItems As Boolean
    ReadOnly Property CanInteract As Boolean
    ReadOnly Property HasItems As Boolean
    ReadOnly Property Items As IEnumerable(Of IItem)
    Sub Consume(item As IItem)
End Interface
