Module Program
    Sub Main(args As String())
        Console.Title = "Key Seeker of SPLORR!!"
#Disable Warning CA1416 ' Validate platform compatibility
        Console.BufferWidth = 120
        Console.BufferHeight = 30
        Console.WindowWidth = 120
        Console.WindowHeight = 30
#Enable Warning CA1416 ' Validate platform compatibility
        Host.Run()
    End Sub
End Module
