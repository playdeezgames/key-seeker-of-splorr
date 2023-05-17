Imports System.Runtime.CompilerServices

Public Module Extensions
    <Extension>
    Public Function Name(itemType As ItemType) As String
        Return itemType.Descriptor.Name
    End Function
End Module
