﻿Imports KSOS.Data

Friend Module InteractFeature
    Friend Function Run(feature As IFeature) As Boolean
        Select Case feature.FeatureType
            Case FeatureType.Shoppe
                Return RunShoppe(feature.Shoppe)
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
    Private Function RunShoppe(shoppe As IShoppe) As Boolean
        Do
            AnsiConsole.Clear()
            Dim prompt As New SelectionPrompt(Of String) With {.Title = $"[olive]{shoppe.Name}[/]"}
            prompt.AddChoice(NeverMindText)
            Dim table As New Dictionary(Of String, ITrade)
            Dim postFix = ""
            For Each trade In shoppe.Trades
                Dim text = $"{trade.FromItemType.Name}(x{trade.FromQuantity}) -> {trade.ToItemType.Name}(x{trade.ToQuantity}){postFix}"
                postFix += " " + ChrW(8)
                table(text) = trade
                prompt.AddChoice(text)
            Next
            Dim answer = AnsiConsole.Prompt(prompt)
            Select Case answer
                Case NeverMindText
                    Exit Do
                Case Else
                    RunTrade(table(answer))
            End Select
        Loop
        Return True
    End Function

    Private Sub RunTrade(trade As ITrade)
        Dim avatar = World.Avatar
        If Not avatar.HasItemQuantity(trade.FromItemType, trade.FromQuantity) Then
            AnsiConsole.MarkupLine($"{avatar.Name} does not have {trade.FromQuantity} {trade.FromItemType.Name}!")
            OkPrompt()
            Return
        End If
        avatar.RemoveItemQuantity(trade.FromItemType, trade.FromQuantity)
        avatar.AddItemQuantity(trade.ToItemType, trade.ToQuantity)
        trade.Complete()
        AnsiConsole.MarkupLine("Trade completed!")
        OkPrompt()
    End Sub
End Module
