Public Interface IWorld
    Sub SetAvatar(character As ICharacter)
    Function CreateLocation(locationType As LocationType) As ILocation
    Function CreateCharacter(characterType As CharacterType, location As ILocation) As ICharacter
    Function CreateItem(itemType As ItemType, quantity As Integer) As IItem
    Function Save(fileName As String) As Boolean
    ReadOnly Property Avatar As ICharacter
    ReadOnly Property Locations As IEnumerable(Of ILocation)
    Function CreateFeature(name As String) As IFeature
End Interface
