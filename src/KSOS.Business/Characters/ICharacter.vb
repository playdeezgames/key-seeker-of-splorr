Public Interface ICharacter
    Inherits IThingie
    Property Location As ILocation
    Property Name As String
    Sub Move(direction As Direction)
    Function Run() As Boolean
    Function MakeAttack(defender As ICharacter) As IEnumerable(Of String)
    Function RollDefend() As Integer
    Function RollAttack() As Integer
    Sub TakeDamage(damage As Integer)
    Sub Kill()
    ReadOnly Property Health As Integer
    ReadOnly Property MaximumHealth As Integer
    ReadOnly Property CharacterType As CharacterType
    ReadOnly Property CanMove As Boolean
    ReadOnly Property CanRun As Boolean
    ReadOnly Property CanFight As Boolean
    ReadOnly Property IsDead As Boolean
    Property XP As Integer
End Interface
