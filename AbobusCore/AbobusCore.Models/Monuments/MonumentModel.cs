using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Monuments
{
    public class MonumentModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("city")]
        public Guid CityId { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("monument_title")]
        public Guid MonumentTitleImageId { get; set; }
    }
}
