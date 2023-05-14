Friend Module InPlay
    Sub Run()
        Dim running As Boolean = True
        Do While running
            running = Navigation.Run()
        Loop
    End Sub
End Module
