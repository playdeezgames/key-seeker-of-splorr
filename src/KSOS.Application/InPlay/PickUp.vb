Friend Module PickUp
    Friend Function Run() As Boolean
        Dim prompt As New SelectionPrompt(Of String) With {.Title = "[olive]Pick Up What?[/]"}
        prompt.AddChoice(NeverMindText)
        Dim table As New Dictionary(Of String, IItem)
        Dim avatar = World.Avatar
        Dim location = avatar.Location
        Dim items = location.Items.ToList()
        Dim postFix = ""
        For index = 0 To items.Count - 1
            Dim text = $"{items(index).Name}(x{items(index).Quantity}){postFix}"
            postFix += " " + ChrW(8)
            table.Add(text, items(index))
            prompt.AddChoice(text)
        Next
        Dim answer = AnsiConsole.Prompt(prompt)
        Select Case answer
            Case NeverMindText
                Return True
            Case Else
                Dim item = table(answer)
                AnsiConsole.MarkupLine($"{avatar.Name} takes {item.Name}(x{item.Quantity}).")
                location.RemoveItem(item)
                avatar.AddItem(item)
        End Select
        Return True
    End Function
End Module
