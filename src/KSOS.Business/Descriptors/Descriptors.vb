Imports System.Runtime.CompilerServices

Friend Module Descriptors
    Private ReadOnly characterTypeDescriptors As IReadOnlyDictionary(Of CharacterType, CharacterTypeDescriptor) =
        New Dictionary(Of CharacterType, CharacterTypeDescriptor) From
        {
            {
                CharacterType.N00b,
                New CharacterTypeDescriptor With
                {
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
                    {
                        {StatisticType.MaximumHealth, 3},
                        {StatisticType.Wounds, 0}
                    }
                }}
        }
    <Extension>
    Friend Function Descriptor(characterType As CharacterType) As CharacterTypeDescriptor
        Return characterTypeDescriptors(characterType)
    End Function
End Module
