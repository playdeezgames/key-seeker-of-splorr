﻿Friend Module MainMenu
    Sub Run()
        Do
            AnsiConsole.Clear()
            Dim prompt As New SelectionPrompt(Of String) With {.Title = $"[olive]{Constants.MainMenuText}:[/]"}
            If World Is Nothing Then
                prompt.AddChoice(StartText)
                prompt.AddChoice(LoadGameText)
                prompt.AddChoice(QuitText)
            Else
                prompt.AddChoice(ContinueText)
                prompt.AddChoice(SaveGameText)
                prompt.AddChoice(AbandonGameText)
            End If
            Select Case AnsiConsole.Prompt(prompt)
                Case AbandonGameText
                    If Confirm("Are you sure you want to abandon the game?") Then
                        World = Nothing
                    End If
                Case ContinueText
                    InPlay.Run()
                Case StartText
                    StartGame.Run()
                Case QuitText
                    If Confirm("Are you sure you want to quit?") Then
                        Exit Do
                    End If
                Case LoadGameText
                    LoadGame.Run()
                Case SaveGameText
                    SaveGame.Run()
            End Select
        Loop
    End Sub
End Module
