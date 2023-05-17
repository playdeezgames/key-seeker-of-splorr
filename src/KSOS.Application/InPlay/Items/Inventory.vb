Friend Module Inventory
    Friend Function Run() As Boolean
        AnsiConsole.Clear()
        Dim avatar = World.Avatar
        Dim prompt As New SelectionPrompt(Of String) With {.Title = $"[olive]{avatar.Name}'s Inventory:[/]"}
        prompt.AddChoice(NeverMindText)
        Dim table As New Dictionary(Of String, IItem)
        Dim postFix = ""
        For Each item In avatar.Items
            Dim text = $"{item.Name}(x{item.Quantity}){postFix}"
            postFix += " " + ChrW(8)
            table(text) = item
            prompt.AddChoices(text)
        Next
        Dim answer = AnsiConsole.Prompt(prompt)
        Select Case answer
            Case NeverMindText
                Return True
            Case Else
                InventoryDetail.Run(table(answer))
                Return True
        End Select
    End Function
End Module
