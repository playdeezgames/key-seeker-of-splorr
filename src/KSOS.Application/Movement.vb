Friend Module Movement
    Friend Function Run() As Boolean
        Dim prompt As New SelectionPrompt(Of String) With {.Title = "[olive]Move Where?[/]"}
        prompt.AddChoice(NeverMindText)
        Dim table = World.Avatar.Location.Routes.ToDictionary(Function(x) x.Direction.Name, Function(x) x.Direction)
        prompt.AddChoices(table.Keys)
        Dim answer = AnsiConsole.Prompt(prompt)
        Select Case answer
            Case NeverMindText
                Return True
            Case Else
                World.Avatar.Move(table(answer))
                Return True
        End Select
    End Function
End Module
