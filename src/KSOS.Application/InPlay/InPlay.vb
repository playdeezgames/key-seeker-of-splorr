Friend Module InPlay
    Sub Run()
        Dim running As Boolean = True
        Do While running
            running = If(World.Avatar.IsDead, GameOver.Run(), Navigation.Run())
        Loop
    End Sub
End Module
