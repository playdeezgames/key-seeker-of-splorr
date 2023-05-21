Imports System.Runtime.CompilerServices
Imports KSOS.Data

Friend Module Constants
    Friend Const AbandonGameText = "Abandon Game"
    Friend Const ConsumeText = "Consume"
    Friend Const ContinueText = "Continue"
    Friend Const EquipmentText = "Equipment"
    Friend Const EquipText = "Equip"
    Friend Const FightText = "Fight"
    Friend Const GameMenuText = "Game Menu"
    Friend Const InteractText = "Interact..."
    Friend Const InventoryText = "Inventory"
    Friend Const LoadGameText = "Load Game..."
    Friend Const MainMenuText = "Main Menu"
    Friend Const MoveText = "Move..."
    Friend Const NeverMindText = "Never Mind"
    Friend Const NoText = "No"
    Friend Const OkText = "Ok"
    Friend Const PickUpText = "Pick Up..."
    Friend Const QuitText = "Quit"
    Friend Const RunText = "Run"
    Friend Const SaveGameText = "Save Game..."
    Friend Const StartText = "Start"
    Friend Const StatusText = "Status"
    Friend Const UnequipText = "Unequip"
    Friend Const YesText = "Yes"
    Private ReadOnly directionNames As IReadOnlyDictionary(Of Direction, String) =
        New Dictionary(Of Direction, String) From
        {
            {Direction.North, "North"},
            {Direction.East, "East"},
            {Direction.South, "South"},
            {Direction.West, "West"},
            {Direction.Inside, "In"},
            {Direction.Outside, "Out"},
            {Direction.Up, "Up"},
            {Direction.Down, "Down"}
        }
    <Extension>
    Friend Function DirectionName(direction As Direction) As String
        Return directionNames(direction)
    End Function
    Private ReadOnly routeTypeNames As IReadOnlyDictionary(Of RouteType, String) =
        New Dictionary(Of RouteType, String) From
        {
            {RouteType.ForestPath, "Forest Path"},
            {RouteType.DirtRoad, "Dirt Road"},
            {RouteType.TownGate, "Town Gate"},
            {RouteType.GraveyardPath, "Graveyard Path"},
            {RouteType.GraveyardGate, "Graveyard Gate"},
            {RouteType.RuinsEntrance, "Ruins Entrance"},
            {RouteType.RuinsPath, "Ruins Path"},
            {RouteType.Door, "Door"},
            {RouteType.Stairs, "Stairs"},
            {RouteType.Tunnel, "Tunnel"},
            {RouteType.TowerEntrance, "Tower Entrance"}
        }
    <Extension>
    Friend Function RouteTypeName(routeType As RouteType) As String
        Return routeTypeNames(routeType)
    End Function
    Private ReadOnly locationTypeNames As IReadOnlyDictionary(Of LocationType, String) =
        New Dictionary(Of LocationType, String) From
        {
            {LocationType.ForestCorner, "a forest"},
            {LocationType.Forest, "a forest"},
            {LocationType.ForestCenter, "a forest"},
            {LocationType.Town, "town"},
            {LocationType.TownEdge, "town"},
            {LocationType.Graveyard, "a graveyard"},
            {LocationType.Ruins, "a ruins"},
            {LocationType.Inn, "an inn"},
            {LocationType.Cellar, "a cellar"},
            {LocationType.Sewer, "a sewer"},
            {LocationType.Knackery, "a knackery"},
            {LocationType.Blacksmith, "a forge"},
            {LocationType.Tower, "a tower"}
        }
    <Extension>
    Friend Function LocationTypeName(locationType As LocationType) As String
        Return locationTypeNames(locationType)
    End Function
End Module
