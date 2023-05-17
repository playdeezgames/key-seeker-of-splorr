Friend Module Fight
    Friend Function Run() As Boolean
        Dim avatar = World.Avatar
        RunAttack(avatar, avatar.Location.Enemies(avatar).First)
        Dim index = 1
        For Each enemy In avatar.Location.Enemies(avatar)
            RunAttack(enemy, avatar, index)
            index += 1
        Next
        Return True
    End Function

    Private Sub RunAttack(attacker As ICharacter, defender As ICharacter, Optional counter As Integer = 0)
        If attacker.IsDead OrElse defender.IsDead Then
            Return
        End If
        AnsiConsole.Clear()
        Dim lines As IEnumerable(Of String) = attacker.MakeAttack(defender, counter)
        For Each line In lines
            AnsiConsole.MarkupLine(line)
        Next
        OkPrompt()
    End Sub
End Module
