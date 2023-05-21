Friend Module InventoryDetail
    Friend Sub Run(item As IItem)
        AnsiConsole.Clear()
        Dim prompt As New SelectionPrompt(Of String) With {.Title = $"[olive]{item.Name}(x{item.Quantity})[/]"}
        prompt.AddChoice(NeverMindText)
        If item.CanEquip Then
            prompt.AddChoice(EquipText)
        End If
        If item.CanUse(World.Avatar) Then
            prompt.AddChoice(item.UseVerb)
        End If
        Dim answer = AnsiConsole.Prompt(prompt)
        Select Case answer
            Case NeverMindText
                Return
            Case EquipText
                RunEquip(item)
            Case Else
                If answer = item.UseVerb Then
                    RunUse(item)
                Else
                    Throw New NotImplementedException
                End If
        End Select
    End Sub

    Private Sub RunUse(item As IItem)
        item.Use(World.Avatar)
        Message.Run()
    End Sub

    Private Sub RunEquip(item As IItem)
        World.Avatar.Equip(item)
        Message.Run()
    End Sub
End Module
