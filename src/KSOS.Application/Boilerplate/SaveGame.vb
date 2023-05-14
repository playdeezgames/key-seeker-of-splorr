Friend Module SaveGame
    Friend Sub Run()
        Dim fileName = AnsiConsole.Ask("[olive]Save Game As...[/]", "")
        If World.Save(fileName) Then
            AnsiConsole.MarkupLine("[green]Saved game![/]")
        Else
            AnsiConsole.MarkupLine("[red]Saving Game Failed![/]")
        End If
        OkPrompt()
    End Sub
End Module
