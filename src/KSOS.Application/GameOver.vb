Friend Module GameOver
    Friend Function Run() As Boolean
        AnsiConsole.Clear()
        AnsiConsole.MarkupLine("[red]Yer Dead![/]")
        World = Nothing
        OkPrompt()
        Return False
    End Function
End Module
