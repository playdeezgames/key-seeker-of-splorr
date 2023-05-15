Imports System.Runtime.CompilerServices

Friend Module Descriptors
    Private ReadOnly characterTypeDescriptors As IReadOnlyDictionary(Of CharacterType, CharacterTypeDescriptor) =
        New Dictionary(Of CharacterType, CharacterTypeDescriptor) From
        {
            {
                CharacterType.N00b,
                New CharacterTypeDescriptor With
                {
                    .Name = "N00b",
                    .SpawnCount = 0,
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
                    {
                        {StatisticType.MaximumHealth, 3},
                        {StatisticType.Wounds, 0}
                    }
                }
            },
            {
                CharacterType.Blob,
                New CharacterTypeDescriptor With
                {
                    .Name = "Blob",
                    .SpawnCount = 100,
                    .SpawnLocations = New List(Of LocationType) From {LocationType.Forest},
                    .Statistics = New Dictionary(Of StatisticType, Integer) From
                    {
                        {StatisticType.MaximumHealth, 1},
                        {StatisticType.Wounds, 0}
                    }
                }
            }
        }
    <Extension>
    Friend Function Descriptor(characterType As CharacterType) As CharacterTypeDescriptor
        Return characterTypeDescriptors(characterType)
    End Function
End Module
