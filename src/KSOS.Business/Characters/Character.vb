﻿Friend Class Character
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

    Public Function MakeAttack(defender As ICharacter) As IEnumerable(Of String) Implements ICharacter.MakeAttack
        Dim attackRoll As Integer = RollAttack()
        Dim defendRoll As Integer = defender.RollDefend()
        Dim lines As New List(Of String) From {
            $"{Name} attacks {defender.Name}!",
            $"{Name} rolls an attack of {attackRoll}.",
            $"{defender.Name} rolls a defend of {defendRoll}."
        }
        If attackRoll > defendRoll Then
            Dim damage = attackRoll - defendRoll
            lines.Add($"{Name} hits {defender.Name} for {damage} damage.")
            defender.TakeDamage(damage)
            If defender.IsDead Then
                lines.Add($"{Name} kills {defender.Name}!")
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
        'TODO: loot drops
        Location.RemoveCharacter(Me)
        WorldData.Characters(Id) = Nothing
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
End Class
