Friend Module InventoryDetail
    Friend Sub Run(item As IItem)
        AnsiConsole.Clear()
        AnsiConsole.MarkupLine($"{item.Name}(x{item.Quantity})")
        OkPrompt()
    End Sub
End Module
