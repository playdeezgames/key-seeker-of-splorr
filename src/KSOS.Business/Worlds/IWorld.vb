Public Interface IWorld
    Sub SetAvatar(character As ICharacter)
    Function CreateLocation(locationType As LocationType) As ILocation
    Function CreateCharacter(characterType As String, location As ILocation) As ICharacter
    Function CreateItem(itemType As String, quantity As Integer) As IItem
    Function Save(fileName As String) As Boolean
    ReadOnly Property Avatar As ICharacter
    ReadOnly Property Locations As IEnumerable(Of ILocation)
    Function CreateFeature(name As String, featureType As String) As IFeature
    ReadOnly Property Characters As IEnumerable(Of ICharacter)
End Interface
