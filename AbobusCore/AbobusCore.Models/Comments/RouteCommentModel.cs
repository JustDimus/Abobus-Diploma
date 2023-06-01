using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Comments
{
    public class RouteCommentModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("route")]
        public Guid RouteId { get; set; }
        [JsonProperty("owner")]
        public Guid OwnerId { get; set; }
        [JsonProperty("comment")]
        public string CommentText { get; set; }
    }
}
