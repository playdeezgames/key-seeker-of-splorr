Friend Module InventoryDetail
    Friend Sub Run(item As IItem)
        AnsiConsole.Clear()
        Dim prompt As New SelectionPrompt(Of String) With {.Title = $"[olive]{item.Name}(x{item.Quantity})[/]"}
        prompt.AddChoice(NeverMindText)
        If item.CanHeal Then
            prompt.AddChoice(ConsumeText)
        End If
        If item.CanEquip Then
            prompt.AddChoice(EquipText)
        End If
        If item.CanUse(World.Avatar) Then
            prompt.AddChoice(UseText)
        End If
        Dim answer = AnsiConsole.Prompt(prompt)
        Select Case answer
            Case NeverMindText
                Return
            Case ConsumeText
                RunConsume(item)
            Case EquipText
                RunEquip(item)
            Case UseText
                RunUse(item)
            Case Else
                Throw New NotImplementedException
        End Select
    End Sub

    Private Sub RunUse(item As IItem)
        Throw New NotImplementedException()
    End Sub

    Private Sub RunEquip(item As IItem)
        World.Avatar.Equip(item)
        Message.Run()
    End Sub

    Private Sub RunConsume(item As IItem)
        Dim avatar = World.Avatar
        AnsiConsole.MarkupLine($"{avatar.Name} consumes {item.Name}")
        avatar.Consume(item)
        AnsiConsole.MarkupLine($"{avatar.Name} has {avatar.Health} health")
        OkPrompt()
    End Sub
End Module
