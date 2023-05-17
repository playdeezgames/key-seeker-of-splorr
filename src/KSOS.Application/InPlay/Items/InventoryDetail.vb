Friend Module InventoryDetail
    Friend Sub Run(item As IItem)
        AnsiConsole.Clear()
        Dim prompt As New SelectionPrompt(Of String) With {.Title = $"[olive]{item.Name}(x{item.Quantity})[/]"}
        prompt.AddChoice(NeverMindText)
        Dim answer = AnsiConsole.Prompt(prompt)
        Select Case answer
            Case NeverMindText
                Return
            Case Else
                Throw New NotImplementedException
        End Select
    End Sub
End Module
