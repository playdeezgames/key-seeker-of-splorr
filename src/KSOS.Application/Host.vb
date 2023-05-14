Public Class Host
    Const OkText = "Ok"
    Public Sub Run()
        Console.Title = "Key Seeker of SPLORR!!"
        AnsiConsole.Clear()
        Dim figlet As New FigletText("Key Seeker of SPLORR!!") With {.Color = Color.Fuchsia, .Justification = Justify.Center}
        AnsiConsole.Write(figlet)
        Dim prompt As New SelectionPrompt(Of String) With {.Title = ""}
        prompt.AddChoice(OkText)
        AnsiConsole.Prompt(prompt)
    End Sub
End Class
