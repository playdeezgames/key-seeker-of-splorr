Friend Module InteractFeatures
    Friend Function Run() As Boolean
        Dim features = World.Avatar.Location.Features
        Dim prompt As New SelectionPrompt(Of String) With {.Title = "[olive]Interact With...[/]"}
        prompt.AddChoice(NeverMindText)
        Dim postFix = ""
        Dim table As New Dictionary(Of String, IFeature)
        For Each feature In features
            Dim text = $"{feature.Name}{postFix}"
            postFix += " " + ChrW(8)
            table(text) = feature
            prompt.AddChoice(feature.Name)
        Next
        Dim answer = AnsiConsole.Prompt(prompt)
        Select Case answer
            Case NeverMindText
                Return True
            Case Else
                Return InteractFeature.Run(table(answer))
        End Select
    End Function
End Module
