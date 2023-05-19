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

    Public Function GetStatistic(statisticType As StatisticType) As Integer Implements ICharacter.GetStatistic
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
            AddMessage($"{Name} cannot go that way!")
            Return
        End If
        If route.RequiredItemType.HasValue AndAlso Not HasItemQuantity(route.RequiredItemType.Value, 1) Then
            AddMessage($"{Name} needs a {route.RequiredItemType.Value.Descriptor.Name} to go that way!")
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

    Public Sub MakeAttack(defender As ICharacter, index As Integer) Implements ICharacter.MakeAttack
        Dim attackRoll As Integer = RollAttack()
        Dim defendRoll As Integer = defender.RollDefend()
        WearWeapon(attackRoll)
        defender.WearArmor(defendRoll)
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
        AddMessage(lines.ToArray)
        defender.AddMessage(lines.ToArray)
    End Sub

    Public Function RollDefend() As Integer Implements ICharacter.RollDefend
        Dim dice = GetStatistic(StatisticType.Defend)
        Dim maximumRoll = GetStatistic(StatisticType.MaximumDefend)
        For Each armor In EquippedItems.Where(Function(x) x.IsArmor)
            dice += armor.Defend
            maximumRoll += armor.MaximumDefend
        Next
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
        For Each weapon In EquippedItems.Where(Function(x) x.IsWeapon)
            dice += weapon.Attack
            maximumRoll += weapon.MaximumAttack
        Next
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

    Public Sub AddMessage(ParamArray lines() As String) Implements ICharacter.AddMessage
        If WorldData.CharacterIndex.HasValue AndAlso Id = WorldData.CharacterIndex.Value Then
            WorldData.Messages.Add(New MessageData With
                                   {
                                    .Lines = lines.ToList
                                   })
        End If
    End Sub

    Public Sub DismissMessage() Implements ICharacter.DismissMessage
        If HasMessages Then
            WorldData.Messages.RemoveAt(0)
        End If
    End Sub

    Public Sub Train(trainingStatistic As ITrainingStatistic) Implements ICharacter.Train
        Dim cost = trainingStatistic.CalculateCost(Me)
        If XP < cost Then
            AddMessage($"{Name} does not have {cost} XP!")
            Return
        End If
        XP -= cost
        SetStatistic(trainingStatistic.StatisticType, GetStatistic(trainingStatistic.StatisticType) + 1)
        AddMessage(
            $"{Name} spends {cost} XP, and now has {XP} XP.",
            $"{Name} now has a {trainingStatistic.StatisticType.Name} of {GetStatistic(trainingStatistic.StatisticType)}.")
    End Sub

    Public Sub Equip(item As IItem) Implements ICharacter.Equip
        If Not HasItem(item) OrElse Not item.CanEquip Then
            AddMessage($"{Name} cannot equip that {item.Name}.")
            Return
        End If
        Dim equipSlot = item.ItemType.Descriptor.EquipSlot.Value
        If HasEquipment(equipSlot) Then
            Unequip(equipSlot)
        End If
        RemoveItem(item)
        CharacterData.Equipment(equipSlot) = item.Id
        AddMessage($"{Name} equips {item.Name} to {equipSlot.Name}.")
    End Sub

    Public Function HasEquipment(equipSlot As EquipSlot) As Boolean Implements ICharacter.HasEquipment
        Return CharacterData.Equipment.ContainsKey(equipSlot)
    End Function

    Public Sub Unequip(equipSlot As EquipSlot) Implements ICharacter.Unequip
        If Not HasEquipment(equipSlot) Then
            AddMessage($"{Name} has nothing equipped for {equipSlot.Name}.")
            Return
        End If
        Dim item = Equipment(equipSlot)
        AddMessage($"{Name} unequips {item.Name} from {equipSlot.Name}.")
        CharacterData.Equipment.Remove(equipSlot)
        AddItem(item)
    End Sub

    Public Function Equipment(equipSlot As EquipSlot) As IItem Implements ICharacter.Equipment
        If Not HasEquipment(equipSlot) Then
            Return Nothing
        End If
        Return New Item(WorldData, CharacterData.Equipment(equipSlot))
    End Function

    Public Sub WearWeapon(wear As Integer) Implements ICharacter.WearWeapon
        Dim weapons = EquippedItems.Where(Function(x) x.IsWeapon AndAlso Not x.IsBroken)
        While weapons.Any AndAlso wear > 0
            Dim weapon = RNG.FromEnumerable(weapons)
            weapon.DoWear(1)
            wear -= 1
            If weapon.IsBroken Then
                AddMessage($"{Name}'s {weapon.Name} broke!")
                weapons = EquippedItems.Where(Function(x) x.IsWeapon AndAlso Not x.IsBroken)
            End If
        End While
        CleanUpEquipment()
    End Sub

    Private Sub CleanUpEquipment()
        Dim equipSlots = EquippedSlots.Where(Function(x) Equipment(x).IsBroken)
        For Each equipSlot In equipSlots
            CharacterData.Equipment.Remove(equipSlot)
        Next
    End Sub

    Public Sub WearArmor(wear As Integer) Implements ICharacter.WearArmor
        Dim armors = EquippedItems.Where(Function(x) x.IsArmor AndAlso Not x.IsBroken)
        While armors.Any AndAlso wear > 0
            Dim weapon = RNG.FromEnumerable(armors)
            weapon.DoWear(1)
            wear -= 1
            If weapon.IsBroken Then
                AddMessage($"{Name}'s {weapon.Name} broke!")
                armors = EquippedItems.Where(Function(x) x.IsArmor AndAlso Not x.IsBroken)
            End If
        End While
        CleanUpEquipment()
    End Sub

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

    Public ReadOnly Property HasMessages As Boolean Implements ICharacter.HasMessages
        Get
            Return WorldData.CharacterIndex.HasValue AndAlso Id = WorldData.CharacterIndex.Value AndAlso WorldData.Messages.Any
        End Get
    End Property

    Public ReadOnly Property NextMessage As IMessage Implements ICharacter.NextMessage
        Get
            If Not HasMessages Then
                Return Nothing
            End If
            Return New NextMessage(WorldData)
        End Get
    End Property

    Public ReadOnly Property HasAnyEquipment As Boolean Implements ICharacter.HasAnyEquipment
        Get
            Return CharacterData.Equipment.Any
        End Get
    End Property

    Public ReadOnly Property EquippedSlots As IEnumerable(Of EquipSlot) Implements ICharacter.EquippedSlots
        Get
            Return CharacterData.Equipment.Keys
        End Get
    End Property

    Public ReadOnly Property EquippedItems As IEnumerable(Of IItem) Implements ICharacter.EquippedItems
        Get
            Return EquippedSlots.Select(Function(x) Equipment(x))
        End Get
    End Property
End Class
