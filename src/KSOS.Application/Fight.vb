Friend Module Fight
    Friend Function Run() As Boolean
        Dim avatar = World.Avatar
        RunAttack(avatar, avatar.Location.Enemies(avatar).First)
        For Each enemy In avatar.Location.Enemies(avatar)
            RunAttack(enemy, avatar)
        Next
        Return True
    End Function

    Private Sub RunAttack(attacker As ICharacter, defender As ICharacter)
        If attacker.IsDead OrElse defender.IsDead Then
            Return
        End If
        AnsiConsole.Clear()
        Dim lines As IEnumerable(Of String) = attacker.MakeAttack(defender)
        For Each line In lines
            AnsiConsole.MarkupLine(line)
        Next
        OkPrompt()
    End Sub
End Module
