Public Module Host
    Const OkText = "Ok"
    Public Sub Run()
        AnsiConsole.Clear()
        Dim figlet As New FigletText("Key Seeker of SPLORR!!") With {.Color = Color.Fuchsia, .Justification = Justify.Center}
        AnsiConsole.Write(figlet)
        Dim prompt As New SelectionPrompt(Of String) With {.Title = ""}
        prompt.AddChoice(OkText)
        AnsiConsole.Prompt(prompt)
    End Sub
End Module
