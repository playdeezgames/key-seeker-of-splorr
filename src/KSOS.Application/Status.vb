Friend Module Status
    Friend Function Run() As Boolean
        Dim character = World.Avatar
        AnsiConsole.Clear()
        AnsiConsole.MarkupLine($"[blue]{character.Name}'s Status:[/]")
        AnsiConsole.MarkupLine($"Health : {character.Health}/{character.MaximumHealth}")
        OkPrompt()
        Return True
    End Function
End Module
