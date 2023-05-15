﻿Public Interface ICharacter
    Inherits IThingie
    Property Location As ILocation
    Property Name As String
    Sub Move(direction As Direction)
    Function Run() As Boolean
    ReadOnly Property Health As Integer
    ReadOnly Property MaximumHealth As Integer
    ReadOnly Property CharacterType As CharacterType
    ReadOnly Property CanMove As Boolean
    ReadOnly Property CanRun As Boolean
End Interface
