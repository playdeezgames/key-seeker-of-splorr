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
    Private ReadOnly Property Wounds As Integer
        Get
            Return GetStatistic(StatisticType.Wounds)
        End Get
    End Property

    Private Function GetStatistic(statisticType As StatisticType) As Integer
        If CharacterData.Statistics.ContainsKey(statisticType) Then
            Return CharacterData.Statistics(statisticType)
        End If
        Return CharacterType.Descriptor.Statistics(statisticType)
    End Function

    Public ReadOnly Property Health As Integer Implements ICharacter.Health
        Get
            Return Math.Max(MaximumHealth - Wounds, 0)
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
            Return Location.HasRoutes AndAlso Location.Enemies(Me).Any
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
End Class
