Public Interface IWorld
    Sub SetAvatar(character As ICharacter)
    Function CreateLocation(locationType As LocationType) As ILocation
    Function CreateCharacter(characterType As CharacterType, location As ILocation) As ICharacter
    Function Save(fileName As String) As Boolean
    ReadOnly Property Avatar As ICharacter
    ReadOnly Property Locations As IEnumerable(Of ILocation)
End Interface
