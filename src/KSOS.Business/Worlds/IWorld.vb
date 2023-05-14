Public Interface IWorld
    Sub SetAvatar(character As ICharacter)
    Function CreateLocation(locationType As LocationType) As ILocation
    Function CreateCharacter(location As ILocation) As ICharacter
    ReadOnly Property Avatar As ICharacter
End Interface
