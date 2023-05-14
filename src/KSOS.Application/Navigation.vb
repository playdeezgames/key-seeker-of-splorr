Friend Module Navigation
    Function Run() As Boolean
        AnsiConsole.Clear()
        Dim avatar = World.Avatar
        AnsiConsole.MarkupLine($"Character Id: {avatar.Id}")
        Dim location = avatar.Location
        AnsiConsole.MarkupLine($"Location Id: {location.Id}")
        Dim prompt As New SelectionPrompt(Of String) With {.Title = "[olive]Now What?[/]"}
        prompt.AddChoice(GameMenuText)
        Select Case AnsiConsole.Prompt(prompt)
            Case GameMenuText
                Return False
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Module
