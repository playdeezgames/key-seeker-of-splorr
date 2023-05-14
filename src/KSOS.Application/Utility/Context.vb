Friend Module Context
    Friend World As IWorld
    Friend Function LoadWorld(fileName As String) As Boolean
        World = KSOS.Business.World.Load(fileName)
        Return World IsNot Nothing
    End Function
End Module
