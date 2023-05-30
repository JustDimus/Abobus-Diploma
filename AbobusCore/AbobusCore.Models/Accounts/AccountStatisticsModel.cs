using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Accounts
{
    public class AccountStatisticsModel
    {
        [JsonProperty("routes_count")]
        public int RoutesCount { get; set; }
        [JsonProperty("visited_cities_count")]
        public int VisitedCitiesCount { get; set; }
        [JsonProperty("friends_count")]
        public int FriendsCount { get; set; }
        [JsonProperty("passed_distance")]
        public int PassedDistance { get; set; }
        [JsonProperty("distance_unit")]
        public string DistanceUnit { get; set; }
    }
}
