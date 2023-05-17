Imports System.Threading

Friend Module InventoryDetail
    Friend Sub Run(item As IItem)
        AnsiConsole.Clear()
        Dim prompt As New SelectionPrompt(Of String) With {.Title = $"[olive]{item.Name}(x{item.Quantity})[/]"}
        prompt.AddChoice(NeverMindText)
        If item.CanHeal Then
            prompt.AddChoice(ConsumeText)
        End If
        Dim answer = AnsiConsole.Prompt(prompt)
        Select Case answer
            Case NeverMindText
                Return
            Case ConsumeText
                RunConsume(item)
            Case Else
                Throw New NotImplementedException
        End Select
    End Sub

    Private Sub RunConsume(item As IItem)
        Dim avatar = World.Avatar
        AnsiConsole.MarkupLine($"{avatar.Name} consumes {item.Name}")
        avatar.Consume(item)
        AnsiConsole.MarkupLine($"{avatar.Name} has {avatar.Health} health")
        OkPrompt()
    End Sub
End Module
