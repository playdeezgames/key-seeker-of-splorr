Friend Module Status
    Friend Function Run() As Boolean
        Dim character = World.Avatar
        AnsiConsole.Clear()
        AnsiConsole.MarkupLine($"[blue]{character.Name}'s Status:[/]")
        AnsiConsole.MarkupLine($"Attack : {character.MaximumAttack}")
        AnsiConsole.MarkupLine($"Defend : {character.MaximumDefend}")
        AnsiConsole.MarkupLine($"Health : {character.Health}/{character.MaximumHealth}")
        AnsiConsole.MarkupLine($"XP : {character.XP}")
        OkPrompt()
        Return True
    End Function
End Module
