Imports KSOS.Data

Friend Module Equipment
    Friend Function Run() As Boolean
        Dim avatar = World.Avatar
        Do
            Dim prompt As New SelectionPrompt(Of String) With {.Title = $"[olive]{avatar.Name}'s Equipment:[/]"}
            prompt.AddChoice(NeverMindText)
            Dim table As New Dictionary(Of String, String)
            For Each equipSlot In avatar.EquippedSlots
                Dim text = $"{equipSlot.EquipSlotName}: {avatar.Equipment(equipSlot).Name}"
                table.Add(text, equipSlot)
            Next
            prompt.AddChoices(table.Keys)
            Dim answer = AnsiConsole.Prompt(prompt)
            Select Case answer
                Case NeverMindText
                    Exit Do
                Case Else
                    RunEquipSlot(table(answer))
            End Select
        Loop
        Return True
    End Function

    Private Sub RunEquipSlot(equipSlot As String)
        Dim avatar = World.Avatar
        Dim item = avatar.Equipment(equipSlot)
        AnsiConsole.MarkupLine($"Equip Slot: {equipSlot.EquipSlotName}")
        AnsiConsole.MarkupLine($"Item: {item.Name}")
        If item.IsWeapon Then
            AnsiConsole.MarkupLine($"Attack: +{item.Attack}/+{item.MaximumAttack}")
        End If
        If item.IsArmor Then
            AnsiConsole.MarkupLine($"Defend: +{item.Defend}/+{item.MaximumDefend}")
        End If
        AnsiConsole.MarkupLine($"Durability: {item.Durability}/{item.MaximumDurability}")
        Dim prompt As New SelectionPrompt(Of String) With {.Title = $"[olive]Now What?[/]"}
        prompt.AddChoice(NeverMindText)
        prompt.AddChoice(UnequipText)
        Select Case AnsiConsole.Prompt(prompt)
            Case NeverMindText
                Return
            Case UnequipText
                avatar.Unequip(equipSlot)
                Message.Run()
            Case Else
                Throw New NotImplementedException
        End Select
    End Sub
End Module
