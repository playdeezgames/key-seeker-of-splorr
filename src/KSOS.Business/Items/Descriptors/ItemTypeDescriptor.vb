Public Class ItemTypeDescriptor
    Public Property Name As String
    Public Property Stacks As Boolean
    Public Property Statistics As IReadOnlyDictionary(Of String, Integer)
    Public Property EquipSlot As String
    Public Property CanUse As Func(Of ICharacter, Boolean)
    Public Property Use As Action(Of ICharacter)
End Class
