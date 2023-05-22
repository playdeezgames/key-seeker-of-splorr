Friend Module CombatRun
    Friend Function Run() As Boolean
        Dim avatar = World.Avatar
        If avatar.Run() Then
            AnsiConsole.MarkupLine($"{avatar.Name} runs away!")
            OkPrompt()
        Else
            AnsiConsole.MarkupLine($"{avatar.Name} fails to run away!")
            OkPrompt()
            RunCounterAttacks(avatar)
        End If
        Return True
    End Function
End Module
