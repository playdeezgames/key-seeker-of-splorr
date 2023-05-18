Module Message
    Friend Function Run() As Boolean
        AnsiConsole.Clear()
        Dim nextMessage = World.Avatar.NextMessage
        For Each line In nextMessage.Lines
            AnsiConsole.MarkupLine(line)
        Next
        OkPrompt()
        World.Avatar.DismissMessage()
        Return True
    End Function
End Module
