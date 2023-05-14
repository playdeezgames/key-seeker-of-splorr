﻿Friend Module Navigation
    Function Run() As Boolean
        AnsiConsole.Clear()
        Dim avatar = World.Avatar
        AnsiConsole.MarkupLine($"Character Id: {avatar.Id}")
        Dim location = avatar.Location
        AnsiConsole.MarkupLine($"Exits: {String.Join(", ", location.Routes.Select(Function(x) $"{x.RouteType.Name} going {x.Direction.Name}"))}")
        Dim prompt As New SelectionPrompt(Of String) With {.Title = "[olive]Now What?[/]"}
        If location.HasRoutes Then
            prompt.AddChoice(MoveText)
        End If
        prompt.AddChoice(GameMenuText)
        Select Case AnsiConsole.Prompt(prompt)
            Case GameMenuText
                Return False
            Case MoveText
                Return Movement.Run()
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Module
