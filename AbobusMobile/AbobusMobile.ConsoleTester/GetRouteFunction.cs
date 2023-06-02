using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.ConsoleTester
{
    public class GetRouteFunction
    {
        public static async Task Run()
        {
            var apiKey = "AIzaSyDcwzqpG2Ejl3VaxedLS2KLjvwYAVOS7cA";

            Position origin = new Position(26.984373f, 49.445339f);
            Position oasis = new Position(26.98131135399367f, 49.433515135938336f);
            Position center = new Position(26.9797432718307f, 49.42682414587117f);

            Position railwayStation = new Position(27.01012780466215f, 49.41686922223756f);

            var requestSource = BuildRoute(
                apiKey,
                origin,
                railwayStation,
                oasis,
                center);

            var httpClient = new HttpClient();

            var result = await httpClient.GetAsync(requestSource);

            using (var file = File.Create("D:\\TestData.json"))
            {
                await result.Content.CopyToAsync(file);
            }

            string BuildRoute(
                string apiKey,
                Position startPosition,
                Position destinationPosition,
                params Position[] positions)
            {
                StringBuilder result = new StringBuilder();

                result.Append("https://maps.googleapis.com/maps/api/directions/json");

                result.Append($"?origin={startPosition.Latitude.ToString(CultureInfo.InvariantCulture)}," +
                    $"{startPosition.Longitude.ToString(CultureInfo.InvariantCulture)}");
                result.Append($"&destination={destinationPosition.Latitude.ToString(CultureInfo.InvariantCulture)}," +
                    $"{destinationPosition.Longitude.ToString(CultureInfo.InvariantCulture)}");

                if (positions.Any())
                {
                    result
                        .Append($"&waypoints={string.Join("|", positions.Select(i => $"{i.Latitude.ToString(CultureInfo.InvariantCulture)}," +
                        $"{i.Longitude.ToString(CultureInfo.InvariantCulture)}"))}");
                }

                result.Append($"&mode=walking");

                result.Append($"&key={apiKey}");

                return result.ToString();
            }
        }
    }
    class Position
    {
        public Position(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
