Public Module Host
    Public Sub Run()
        AnsiConsole.Clear()
        Dim figlet As New FigletText("Key Seeker of SPLORR!!") With {.Color = Color.Fuchsia, .Justification = Justify.Center}
        AnsiConsole.Write(figlet)
        OkPrompt()
    End Sub
End Module
