using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Routes
{
    public class RouteDetailsModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("city")]
        public Guid CityId { get; set; }
        [JsonProperty("creator_id")]
        public Guid CreatorId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("distance")]
        public int Distance { get; set; }
        [JsonProperty("distance_unit")]
        public string DistanceUnit { get; set; }
        [JsonProperty("route_resource")]
        public Guid RouteResourceId { get; set; }
        [JsonProperty("route_image")]
        public Guid RouteImageId { get; set; }
    }
}
