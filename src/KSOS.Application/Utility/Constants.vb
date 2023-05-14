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
    Friend Const YesText = "Yes"
    Private ReadOnly directionNames As IReadOnlyDictionary(Of Direction, String) =
        New Dictionary(Of Direction, String) From
        {
            {Direction.North, "North"},
            {Direction.East, "East"},
            {Direction.South, "South"},
            {Direction.West, "West"},
            {Direction.Inside, "In"},
            {Direction.Outside, "Out"}
        }
    Private ReadOnly routeTypeNames As IReadOnlyDictionary(Of RouteType, String) =
        New Dictionary(Of RouteType, String) From
        {
            {RouteType.ForestPath, "Forest Path"},
            {RouteType.DirtRoad, "Dirt Road"},
            {RouteType.TownGate, "Town Gate"}
        }
    <Extension>
    Friend Function Name(routeType As RouteType) As String
        Return routeTypeNames(routeType)
    End Function
    <Extension>
    Friend Function Name(direction As Direction) As String
        Return directionNames(direction)
    End Function
End Module
