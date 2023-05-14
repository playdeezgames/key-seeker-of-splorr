Friend Module LoadGame
    Friend Sub Run()
        Dim fileName = AnsiConsole.Ask("[olive]Load Game From...[/]", "")
        If LoadWorld(fileName) Then
            AnsiConsole.MarkupLine("[green]Loaded game![/]")
            OkPrompt()
            InPlay.Run()
        Else
            AnsiConsole.MarkupLine("[red]Loading Game Failed![/]")
            OkPrompt()
        End If
    End Sub
End Module
