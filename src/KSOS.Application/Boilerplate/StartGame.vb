Friend Module StartGame
    Friend Sub Run()
        World = New World()
        World.Avatar.Name = AnsiConsole.Ask("[olive]Character Name?[/]", "N00b")
        InPlay.Run()
    End Sub
End Module
