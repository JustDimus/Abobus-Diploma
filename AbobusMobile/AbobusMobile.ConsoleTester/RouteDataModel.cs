using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.ConsoleTester
{

    public class RouteCoordinateModel
    {
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }
    }

    public class RoutePointModel
    {
        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("destination_here")]
        public bool IsDestination { get; set; }

        [JsonProperty("monument")]
        public Guid? MonumentId { get; set; }

        [JsonProperty("coordinate")]
        public RouteCoordinateModel Coordinate { get; set; }
    }

    public class RouteMonumentModel
    {
        [JsonProperty("id")]
        public Guid MonumentId { get; set; }

        [JsonProperty("coordinate")]
        public RouteCoordinateModel Coordinate { get; set; }
    }

    public class RouteDataModel
    {
        [JsonProperty("route")]
        public Guid RouteId { get; set; }

        [JsonProperty("points")]
        public List<RoutePointModel> RoutePoints { get; set; }

        [JsonProperty("monuments")]
        public List<RouteMonumentModel> RouteMonuments { get; set; }
    }
}
