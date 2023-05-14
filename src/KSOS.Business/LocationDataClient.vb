Public MustInherit Class LocationDataClient
    Inherits WorldDataClient
    Private ReadOnly _locationId As Integer
    Protected ReadOnly Property LocationData As LocationData
        Get
            Return WorldData.Locations(_locationId)
        End Get
    End Property
    Protected Sub New(data As WorldData, locationId As Integer)
        MyBase.New(data)
        _locationId = locationId
    End Sub
End Class
