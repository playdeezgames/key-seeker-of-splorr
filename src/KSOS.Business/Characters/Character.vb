Friend Class Character
    Inherits CharacterDataClient
    Implements ICharacter
    Public Sub New(data As WorldData, characterId As Integer)
        MyBase.New(data, characterId)
        Id = characterId
    End Sub
    Public ReadOnly Property Id As Integer Implements IThingie.Id
    Public Property Location As ILocation Implements ICharacter.Location
        Get
            Return New Location(WorldData, CharacterData.LocationId)
        End Get
        Set(value As ILocation)
            If value.Id <> CharacterData.LocationId Then
                Location.RemoveCharacter(Me)
                CharacterData.LocationId = value.Id
                Location.AddCharacter(Me)
            End If
        End Set
    End Property

    Public Property Name As String Implements ICharacter.Name
        Get
            Return If(String.IsNullOrEmpty(CharacterData.Name), CharacterType.Descriptor.Name, CharacterData.Name)
        End Get
        Set(value As String)
            CharacterData.Name = value
        End Set
    End Property
    Private Property Wounds As Integer
        Get
            Return GetStatistic(StatisticType.Wounds)
        End Get
        Set(value As Integer)
            SetStatistic(StatisticType.Wounds, Math.Clamp(value, 0, MaximumHealth))
        End Set
    End Property

    Private Sub SetStatistic(statisticType As StatisticType, value As Integer)
        CharacterData.Statistics(statisticType) = value
    End Sub

    Private Function GetStatistic(statisticType As StatisticType) As Integer
        If CharacterData.Statistics.ContainsKey(statisticType) Then
            Return CharacterData.Statistics(statisticType)
        End If
        Return CharacterType.Descriptor.Statistics(statisticType)
    End Function

    Public ReadOnly Property Health As Integer Implements ICharacter.Health
        Get
            Return Math.Clamp(MaximumHealth - Wounds, 0, MaximumHealth)
        End Get
    End Property

    Public ReadOnly Property MaximumHealth As Integer Implements ICharacter.MaximumHealth
        Get
            Return GetStatistic(StatisticType.MaximumHealth)
        End Get
    End Property

    Public ReadOnly Property CharacterType As CharacterType Implements ICharacter.CharacterType
        Get
            Return CharacterData.CharacterType
        End Get
    End Property

    Public ReadOnly Property CanMove As Boolean Implements ICharacter.CanMove
        Get
            Return Location.HasRoutes AndAlso Not Location.Enemies(Me).Any
        End Get
    End Property

    Public ReadOnly Property CanRun As Boolean Implements ICharacter.CanRun
        Get
            Return Location.HasRoutes AndAlso CanFight
        End Get
    End Property

    Public ReadOnly Property CanFight As Boolean Implements ICharacter.CanFight
        Get
            Return Location.Enemies(Me).Any
        End Get
    End Property

    Public ReadOnly Property IsDead As Boolean Implements ICharacter.IsDead
        Get
            Return Health = 0
        End Get
    End Property

    Public Sub Move(direction As Direction) Implements ICharacter.Move
        Dim route As IRoute = Location.GetRoute(direction)
        If route Is Nothing Then
            Return
        End If
        Location = route.Destination
    End Sub

    Public Function Run() As Boolean Implements ICharacter.Run
        Dim exits = New HashSet(Of Direction)(Location.Routes.Select(Function(x) x.Direction)) From {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West
        }
        Dim chosen = RNG.FromEnumerable(exits)
        If Location.HasRoute(chosen) Then
            Move(chosen)
            Return True
        End If
        Return False
    End Function

    Public Function MakeAttack(defender As ICharacter, index As Integer) As IEnumerable(Of String) Implements ICharacter.MakeAttack
        Dim attackRoll As Integer = RollAttack()
        Dim defendRoll As Integer = defender.RollDefend()
        Dim lines As New List(Of String)
        If index > 0 Then
            lines.Add($"Enemy #{index}:")
        End If
        lines.Add($"{Name} attacks {defender.Name}!")
        lines.Add($"{Name} rolls an attack of {attackRoll}.")
        lines.Add($"{defender.Name} rolls a defend of {defendRoll}.")
        If attackRoll > defendRoll Then
            Dim damage = attackRoll - defendRoll
            lines.Add($"{Name} hits {defender.Name} for {damage} damage.")
            defender.TakeDamage(damage)
            If defender.IsDead Then
                lines.Add($"{Name} {KillVerb} {defender.Name}!")
                Dim xp As Integer = defender.XP
                If xp > 0 Then
                    Me.XP += xp
                    lines.Add($"{Name} gains {xp} XP and now has {Me.XP} XP!")
                End If
                defender.Kill()
            Else
                lines.Add($"{defender.Name} has {defender.Health} health left.")
            End If
        Else
            lines.Add($"{Name} misses.")
        End If
        Return lines
    End Function

    Public Function RollDefend() As Integer Implements ICharacter.RollDefend
        Dim dice = GetStatistic(StatisticType.Defend)
        Dim maximumRoll = GetStatistic(StatisticType.MaximumDefend)
        Return DoDiceRoll(dice, maximumRoll)
    End Function

    Private Shared Function DoDiceRoll(dice As Integer, maximumRoll As Integer) As Integer
        Dim roll = 0
        While dice > 0
            dice -= 1
            roll += RNG.RollDice("1d6/6")
        End While
        Return Math.Clamp(roll, 0, maximumRoll)
    End Function

    Public Function RollAttack() As Integer Implements ICharacter.RollAttack
        Dim dice = GetStatistic(StatisticType.Attack)
        Dim maximumRoll = GetStatistic(StatisticType.MaximumAttack)
        Return DoDiceRoll(dice, maximumRoll)
    End Function

    Public Sub TakeDamage(damage As Integer) Implements ICharacter.TakeDamage
        Wounds += damage
    End Sub

    Public Sub Kill() Implements ICharacter.Kill
        If IsAvatar Then
            Return
        End If
        For Each item In Items
            RemoveItem(item)
            Location.AddItem(item)
        Next
        Location.RemoveCharacter(Me)
        WorldData.Characters(Id) = Nothing
    End Sub

    Public Sub AddItem(item As IItem) Implements ICharacter.AddItem
        If item.Stacks Then
            Dim existingItem = Items.FirstOrDefault(Function(x) x.ItemType = item.ItemType)
            If existingItem IsNot Nothing Then
                existingItem.Quantity += item.Quantity
                item.Quantity = 0
            Else
                CharacterData.ItemIds.Add(item.Id)
            End If
        Else
            CharacterData.ItemIds.Add(item.Id)
        End If
    End Sub

    Public Sub RemoveItem(item As IItem) Implements ICharacter.RemoveItem
        CharacterData.ItemIds.Remove(item.Id)
    End Sub

    Public Function HasItemQuantity(itemType As ItemType, quantity As Integer) As Boolean Implements ICharacter.HasItemQuantity
        Return ItemQuantity(itemType) >= quantity
    End Function

    Private Function ItemQuantity(itemType As ItemType) As Integer
        Return Items.Where(Function(x) x.ItemType = itemType).Sum(Function(x) x.Quantity)
    End Function

    Public Sub RemoveItemQuantity(itemType As ItemType, quantity As Integer) Implements ICharacter.RemoveItemQuantity
        quantity = Math.Min(quantity, ItemQuantity(itemType))
        For Each item In Items.Where(Function(x) x.ItemType = itemType)
            If item.Quantity >= quantity Then
                item.Quantity -= quantity
                quantity = 0
                Exit For
            Else
                quantity -= item.Quantity
                item.Quantity = 0
            End If
        Next
        CleanUpItems()
    End Sub

    Private Sub CleanUpItems()
        Dim itemsToRemove = Items.Where(Function(x) x.Quantity <= 0).ToList
        For Each item In itemsToRemove
            RemoveItem(item)
        Next
    End Sub

    Public Sub AddItemQuantity(itemType As ItemType, quantity As Integer) Implements ICharacter.AddItemQuantity
        AddItem(World.CreateItem(itemType, quantity))
    End Sub

    Public Sub Consume(item As IItem) Implements ICharacter.Consume
        If HasItem(item) And item.CanHeal Then
            Wounds -= item.ItemType.Descriptor.Statistics(StatisticType.Healing)
            item.Quantity -= 1
            CleanUpItems()
        End If
    End Sub

    Private Function HasItem(item As IItem) As Boolean
        Return CharacterData.ItemIds.Contains(item.Id)
    End Function

    Private ReadOnly Property IsAvatar As Boolean
        Get
            Return If(WorldData.CharacterIndex = Id, False)
        End Get
    End Property

    Public Property XP As Integer Implements ICharacter.XP
        Get
            Return GetStatistic(StatisticType.XP)
        End Get
        Set(value As Integer)
            SetStatistic(StatisticType.XP, value)
        End Set
    End Property

    Public ReadOnly Property KillVerb As String Implements ICharacter.KillVerb
        Get
            Return CharacterType.Descriptor.KillVerb
        End Get
    End Property

    Public ReadOnly Property World As IWorld Implements ICharacter.World
        Get
            Return New World(WorldData)
        End Get
    End Property

    Public ReadOnly Property CanPickUpItems As Boolean Implements ICharacter.CanPickUpItems
        Get
            Return Not CanFight AndAlso Location.HasItems
        End Get
    End Property

    Public ReadOnly Property CanInteract As Boolean Implements ICharacter.CanInteract
        Get
            Return Location.HasFeatures
        End Get
    End Property

    Public ReadOnly Property HasItems As Boolean Implements ICharacter.HasItems
        Get
            Return CharacterData.ItemIds.Any
        End Get
    End Property

    Private ReadOnly Property Items As IEnumerable(Of IItem) Implements ICharacter.Items
        Get
            Return CharacterData.ItemIds.Select(Function(x) New Item(WorldData, x))
        End Get
    End Property
End Class
