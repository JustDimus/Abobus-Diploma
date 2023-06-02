using AbobusMobile.ConsoleTester;
using Newtonsoft.Json;

//await GetRouteFunction.Run();

var routeModel = new RouteDataModel()
{
    RouteId = Guid.Parse("90E71CB9-9656-414D-BFAA-F15CA702E36A"),
    RouteMonuments = new List<RouteMonumentModel>()
    {
        new RouteMonumentModel()
        {
            MonumentId = Guid.Parse("96532D9E-1C57-4E8B-9CA4-BCE0BE57E2F4"),
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.433394,
                Longitude = 26.981422
            }
        },
        new RouteMonumentModel()
        {
            MonumentId = Guid.Parse("79BCF08F-BEE7-4A18-A865-214BE71F6D8B"),
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.426657,
                Longitude = 26.979630
            }
        },
        new RouteMonumentModel()
        {
            MonumentId = Guid.Parse("993B0275-B124-450A-A4CC-03D122D99D2F"),
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.416926,
                Longitude = 27.009943
            }
        }
    },
    RoutePoints = new List<RoutePointModel>()
    {
        new RoutePointModel()
        {
            Order = 0,
            IsDestination = false,
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.4453377,
                Longitude = 26.9843716,
            }
        },
        new RoutePointModel()
        {
            Order = 1,
            IsDestination = false,
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.4437759,
                Longitude = 26.9879344,
            }
        },
        new RoutePointModel()
        {
            Order = 2,
            IsDestination = false,
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.4349082,
                Longitude = 26.9820159,
            }
        },
        new RoutePointModel()
        {
            Order = 3,
            IsDestination = true,
            MonumentId = Guid.Parse("96532D9E-1C57-4E8B-9CA4-BCE0BE57E2F4"),
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.4335301,
                Longitude = 26.9812418,
            }
        },
        new RoutePointModel()
        {
            Order = 4,
            IsDestination = false,
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.4275709,
                Longitude = 26.9794627,
            }
        },
        new RoutePointModel()
        {
            Order = 5,
            IsDestination = false,
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.4275536,
                Longitude = 26.9796658,
            }
        },
        new RoutePointModel()
        {
            Order = 6,
            IsDestination = true,
            MonumentId = Guid.Parse("79BCF08F-BEE7-4A18-A865-214BE71F6D8B"),
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.4267919,
                Longitude = 26.9797611,
            }
        },
        new RoutePointModel()
        {
            Order = 7,
            IsDestination = false,
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.426771,
                Longitude = 26.9799596,
            }
        },
        new RoutePointModel()
        {
            Order = 8,
            IsDestination = false,
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.4268144,
                Longitude = 26.9803794,
            }
        },
        new RoutePointModel()
        {
            Order = 9,
            IsDestination = false,
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.4267215,
                Longitude = 26.9815378,
            }
        },
        new RoutePointModel()
        {
            Order = 10,
            IsDestination = false,
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.4261655,
                Longitude = 26.981382,
            }
        },
        new RoutePointModel()
        {
            Order = 11,
            IsDestination = false,
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.4173147,
                Longitude = 27.0092328,
            }
        },
        new RoutePointModel()
        {
            Order = 12,
            IsDestination = true,
            MonumentId = Guid.Parse("993B0275-B124-450A-A4CC-03D122D99D2F"),
            Coordinate = new RouteCoordinateModel()
            {
                Latitude = 49.4168853,
                Longitude = 27.010109,
            }
        },
    }
};

var serializedEntity = JsonConvert.SerializeObject(routeModel);

using (var file = File.Create("D:\\Projects\\Abobus-Diploma\\Tools\\Route1Data.json"))
{
    using (StreamWriter writer = new StreamWriter(file))
    {
        await writer.WriteAsync(serializedEntity);
    }
}

// Latitude: 49... Longitude: 26...

// 49.416926, 27.009943

