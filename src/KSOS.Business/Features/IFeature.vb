Public Interface IFeature
    Inherits IThingie
    ReadOnly Property Name As String
    ReadOnly Property FeatureType As FeatureType
    ReadOnly Property Shoppe As IShoppe
End Interface
