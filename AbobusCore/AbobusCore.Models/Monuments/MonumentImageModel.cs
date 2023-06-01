using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Monuments
{
    public class MonumentImageModel
    {
        [JsonProperty("image")]
        public Guid ImageId { get; set; }
    }
}
