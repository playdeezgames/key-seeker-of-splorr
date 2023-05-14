Imports System.Runtime.CompilerServices
Imports KSOS.Data

Friend Module Constants
    Friend Const AbandonGameText = "Abandon Game"
    Friend Const ContinueText = "Continue"
    Friend Const GameMenuText = "Game Menu"
    Friend Const MainMenuText = "Main Menu"
    Friend Const MoveText = "Move..."
    Friend Const NeverMindText = "Never Mind"
    Friend Const NoText = "No"
    Friend Const OkText = "Ok"
    Friend Const QuitText = "Quit"
    Friend Const StartText = "Start"
    Friend Const YesText = "Yes"
    Private ReadOnly directionNames As IReadOnlyDictionary(Of Direction, String) =
        New Dictionary(Of Direction, String) From
        {
            {Direction.North, "North"},
            {Direction.East, "East"},
            {Direction.South, "South"},
            {Direction.West, "West"}
        }
    <Extension>
    Friend Function Name(direction As Direction) As String
        Return directionNames(direction)
    End Function
End Module
