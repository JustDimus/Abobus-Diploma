using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Locations
{
    public class LocationDetailsModel
    {
        [JsonProperty("location_found")]
        public bool LocationFound { get; set; }

        [JsonProperty("city_id")]
        public Guid? CityId { get; set; }

        [JsonProperty("city_name")]
        public string CityName { get; set; }
    }
}
