Imports System.Runtime.CompilerServices
Imports KSOS.Data

Friend Module Constants
    Friend Const AbandonGameText = "Abandon Game"
    Friend Const ContinueText = "Continue"
    Friend Const GameMenuText = "Game Menu"
    Friend Const LoadGameText = "Load Game..."
    Friend Const MainMenuText = "Main Menu"
    Friend Const MoveText = "Move..."
    Friend Const NeverMindText = "Never Mind"
    Friend Const NoText = "No"
    Friend Const OkText = "Ok"
    Friend Const QuitText = "Quit"
    Friend Const SaveGameText = "Save Game..."
    Friend Const StartText = "Start"
    Friend Const StatusText = "Status"
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
    Friend Function Name(direction As Direction) As String
        Return directionNames(direction)
    End Function
    Private ReadOnly routeTypeNames As IReadOnlyDictionary(Of RouteType, String) =
        New Dictionary(Of RouteType, String) From
        {
            {RouteType.ForestPath, "Forest Path"},
            {RouteType.DirtRoad, "Dirt Road"},
            {RouteType.TownGate, "Town Gate"},
            {RouteType.GraveyardPath, "Graveyard Path"},
            {RouteType.GraveyardGate, "Graveyard Gate"}
        }
    <Extension>
    Friend Function Name(routeType As RouteType) As String
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
            {LocationType.Graveyard, "a graveyard"}
        }
    <Extension>
    Friend Function Name(locationType As LocationType) As String
        Return locationTypeNames(locationType)
    End Function
End Module
