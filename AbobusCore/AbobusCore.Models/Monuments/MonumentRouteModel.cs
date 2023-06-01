using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Monuments
{
    public class MonumentRouteModel
    {
        [JsonProperty("monument")]
        public Guid MonumentId { get; set; }
    }
}
