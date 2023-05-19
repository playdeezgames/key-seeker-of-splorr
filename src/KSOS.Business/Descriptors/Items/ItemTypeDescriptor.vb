Public Class ItemTypeDescriptor
    Public Property Name As String
    Public Property Stacks As Boolean
    Public Property Statistics As IReadOnlyDictionary(Of StatisticType, Integer)
    Public Property EquipSlot As EquipSlot?
End Class
