Friend Module TownInitializer
    Const Columns = 3
    Const Rows = 3
    Friend Sub Initialize(world As IWorld)
        Dim maze As New Maze(Of String)(Columns, Rows, mazeDirections)
        maze.Generate()
        Dim mazeLocations(Columns - 1, Rows - 1) As ILocation
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                Dim lt = If(
                    column = 0 OrElse column = Columns - 1 OrElse row = 0 OrElse row = Rows - 1,
                    LocationType.TownEdge,
                    LocationType.Town)
                mazeLocations(column, row) = world.CreateLocation(lt)
            Next
        Next
        For column = 0 To Columns - 1
            For row = 0 To Rows - 1
                Dim cell = maze.GetCell(column, row)
                For Each direction In cell.Directions
                    If cell.GetDoor(direction).Open Then
                        Dim nextColumn = CInt(mazeDirections(direction).DeltaX) + column
                        Dim nextRow = CInt(mazeDirections(direction).DeltaY) + row
                        mazeLocations(column, row).CreateRoute(direction, RouteType.DirtRoad, mazeLocations(nextColumn, nextRow))
                    End If
                Next
            Next
        Next
        Dim forestCenter = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.ForestCenter))
        mazeLocations(Columns \ 2, 0).CreateRoute(Direction.North, RouteType.TownGate, forestCenter)
        mazeLocations(Columns \ 2, Rows - 1).CreateRoute(Direction.South, RouteType.TownGate, forestCenter)
        mazeLocations(0, Rows \ 2).CreateRoute(Direction.West, RouteType.TownGate, forestCenter)
        mazeLocations(Columns - 1, Rows \ 2).CreateRoute(Direction.East, RouteType.TownGate, forestCenter)
        forestCenter.CreateRoute(Direction.Inside, RouteType.TownGate, mazeLocations(Columns \ 2, Rows - 1))
        InitializeInn(world)
        InitializeKnacker(world)
        InitializeYogi(world)
        InitializeBlacksmith(world)
        InitializeHealer(world)
        InitializeElder(world)
    End Sub
    Private Sub InitializeElder(world As IWorld)
        Dim location = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.TownEdge))
        Dim zooperDan = world.CreateFeature("Zooperdan the Elder", FeatureType.Shoppe)
        zooperDan.Shoppe.Name = "Zooperdan's Bargain Town Portal Scrolls"
        zooperDan.Shoppe.AddTrade((ItemType.Jools, 25), (ItemType.TownPortal, 1))
        location.AddFeature(zooperDan)
    End Sub
    Private Sub InitializeHealer(world As IWorld)
        Dim entrance = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.TownEdge AndAlso Not x.HasRoute(Direction.Inside)))
        Dim hut = world.CreateLocation(LocationType.Hut)
        entrance.CreateRoute(Direction.Inside, RouteType.Door, hut)
        hut.CreateRoute(Direction.Outside, RouteType.Door, entrance)
        Dim mårten = world.CreateFeature("Mårten the Nihilist Healer", FeatureType.Shoppe)
        mårten.Shoppe.Name = "Mårten's Apathetic Apothecary"
        mårten.Shoppe.AddTrade((ItemType.Jools, 9), (ItemType.Potion, 1))
        mårten.Shoppe.AddTrade((ItemType.Jools, 3), (ItemType.HolyWater, 1))
        hut.AddFeature(mårten)
    End Sub

    Private Sub InitializeBlacksmith(world As IWorld)
        Dim entrance = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.TownEdge AndAlso Not x.HasRoute(Direction.Inside)))
        Dim blacksmithShoppe = world.CreateLocation(LocationType.Blacksmith)
        entrance.CreateRoute(Direction.Inside, RouteType.Door, blacksmithShoppe)
        blacksmithShoppe.CreateRoute(Direction.Outside, RouteType.Door, entrance)
        Dim samuli = world.CreateFeature("Samuli's Blacksmithery", FeatureType.Shoppe)
        samuli.Shoppe.Name = "Samuli's Wares"
        samuli.Shoppe.AddTrade((ItemType.Jools, 25), (ItemType.Dagger, 1))
        samuli.Shoppe.AddTrade((ItemType.Jools, 50), (ItemType.Shortsword, 1))
        samuli.Shoppe.AddTrade((ItemType.Jools, 75), (ItemType.Broadsword, 1))
        samuli.Shoppe.AddTrade((ItemType.Jools, 100), (ItemType.Longsword, 1))
        samuli.Shoppe.AddTrade((ItemType.Jools, 125), (ItemType.Axe, 1))
        samuli.Shoppe.AddTrade((ItemType.Jools, 50), (ItemType.Shield, 1))
        samuli.Shoppe.AddTrade((ItemType.Jools, 25), (ItemType.Helmet, 1))
        samuli.Shoppe.AddTrade((ItemType.Jools, 50), (ItemType.LeatherArmor, 1))
        samuli.Shoppe.AddTrade((ItemType.Jools, 100), (ItemType.ChainMail, 1))
        samuli.Shoppe.AddTrade((ItemType.Jools, 150), (ItemType.PlateMail, 1))
        blacksmithShoppe.AddFeature(samuli)
    End Sub
    Private Sub InitializeYogi(world As IWorld)
        Dim location = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.TownEdge))
        Dim yogi = world.CreateFeature("Yogi the Bare Bear Yoga Instructor", FeatureType.Trainer)
        yogi.Training.Name = "Yogi The Bare Bear's Personal Training"
        yogi.Training.Add(StatisticType.MaximumHealth, 10)
        location.AddFeature(yogi)
    End Sub
    Private Sub InitializeKnacker(world As IWorld)
        Dim entrance = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.TownEdge AndAlso Not x.HasRoute(Direction.Inside)))
        Dim knackery = world.CreateLocation(LocationType.Knackery)
        entrance.CreateRoute(Direction.Inside, RouteType.Door, knackery)
        knackery.CreateRoute(Direction.Outside, RouteType.Door, entrance)
        Dim dan = world.CreateFeature("""Honest"" Dan the Knacker", FeatureType.Shoppe)
        dan.Shoppe.Name = """Honest"" Dan's Knackery"
        dan.Shoppe.AddTrade((ItemType.SewerRatTail, 3), (ItemType.Jools, 1))
        dan.Shoppe.AddTrade((ItemType.SewerRatTail, 7), (ItemType.Jools, 2))
        dan.Shoppe.AddTrade((ItemType.SkullFragment, 4), (ItemType.Jools, 1))
        dan.Shoppe.AddTrade((ItemType.ZombieTaint, 2), (ItemType.Jools, 1))
        dan.Shoppe.AddTrade((ItemType.Feather, 1), (ItemType.Jools, 1))
        dan.Shoppe.AddTrade((ItemType.GoblinEar, 1), (ItemType.Jools, 2))
        dan.Shoppe.AddTrade((ItemType.OrcTooth, 1), (ItemType.Jools, 3))
        dan.Shoppe.AddTrade((ItemType.CyclopsEyeball, 1), (ItemType.Jools, 5))
        dan.Shoppe.AddTrade((ItemType.EmptyBottle, 2), (ItemType.Jools, 5))
        dan.Shoppe.AddTrade((ItemType.MarbleChunk, 1), (ItemType.Jools, 10))
        knackery.AddFeature(dan)
    End Sub
    Private Sub InitializeInn(world As IWorld)
        Dim entrance = RNG.FromEnumerable(world.Locations.Where(Function(x) x.LocationType = LocationType.TownEdge AndAlso Not x.HasRoute(Direction.Inside)))
        Dim inn = world.CreateLocation(LocationType.Inn)
        entrance.CreateRoute(Direction.Inside, RouteType.Door, inn)
        inn.CreateRoute(Direction.Outside, RouteType.Door, entrance)
        Dim cellar = world.CreateLocation(LocationType.Cellar)
        inn.CreateRoute(Direction.Down, RouteType.Stairs, cellar)
        cellar.CreateRoute(Direction.Up, RouteType.Stairs, inn)
        Dim gurachan = world.CreateFeature("Gurachan The Innkeeper", FeatureType.Shoppe)
        gurachan.Shoppe.Name = "Resting Dog's Face Inn"
        gurachan.Shoppe.AddTrade((ItemType.Jools, 2), (ItemType.Chikkin, 1))
        gurachan.Shoppe.AddTrade((ItemType.CellarRatTail, 5), (ItemType.SewerKey, 1), 1)
        inn.AddFeature(gurachan)
    End Sub
End Module
