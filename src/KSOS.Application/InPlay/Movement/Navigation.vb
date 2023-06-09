﻿Friend Module Navigation
    Function Run() As Boolean
        AnsiConsole.Clear()
        Dim avatar = World.Avatar
        Dim location = avatar.Location
        AnsiConsole.MarkupLine($"{avatar.Name}({avatar.Health}/{avatar.MaximumHealth}) is in {location.LocationType.LocationTypeName}")
        If avatar.CanPickUpItems Then
            AnsiConsole.MarkupLine($"There is stuff on the ground.")
        End If
        Dim enemies = location.Enemies(avatar)
        If enemies.Any Then
            AnsiConsole.MarkupLine($"Enemies: 
    {String.Join("
    ", enemies.Select(Function(x) $"{x.Name}({x.Health}/{x.MaximumHealth})"))}")
        End If
        Dim features = location.Features
        If features.Any Then
            AnsiConsole.MarkupLine($"Features:
    {String.Join("
    ", features.Select(Function(x) $"{x.Name}"))}")
        End If
        AnsiConsole.MarkupLine($"Exits: 
    {String.Join("
    ", location.Routes.Select(Function(x) $"{x.RouteType.RouteTypeName} going {x.Direction.DirectionName}"))}")
        Dim prompt As New SelectionPrompt(Of String) With {.Title = "[olive]Now What?[/]"}
        If avatar.CanMove Then
            prompt.AddChoice(MoveText)
        End If
        If avatar.CanFight Then
            prompt.AddChoice(FightText)
        End If
        If avatar.CanRun Then
            prompt.AddChoice(RunText)
        End If
        If avatar.CanPickUpItems Then
            prompt.AddChoice(PickUpText)
        End If
        If avatar.HasItems Then
            prompt.AddChoice(InventoryText)
        End If
        If avatar.HasAnyEquipment Then
            prompt.AddChoice(EquipmentText)
        End If
        If avatar.CanInteract Then
            prompt.AddChoice(InteractText)
        End If
        prompt.AddChoice(StatusText)
        prompt.AddChoice(GameMenuText)
        Select Case AnsiConsole.Prompt(prompt)
            Case StatusText
                Return Status.Run()
            Case GameMenuText
                Return False
            Case MoveText
                Return Movement.Run()
            Case RunText
                Return CombatRun.Run()
            Case FightText
                Return Fight.Run()
            Case PickUpText
                Return PickUp.Run()
            Case InteractText
                Return InteractFeatures.Run()
            Case InventoryText
                Return Inventory.Run()
            Case EquipmentText
                Return Equipment.Run()
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Module
