Imports System.Runtime.CompilerServices

Public Module Extensions
    <Extension>
    Public Function ItemTypeName(itemType As String) As String
        Return itemType.ItemTypeDescriptor.Name
    End Function
End Module
