using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Comments
{
    public class MonumentCommentModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("monument")]
        public Guid MonumentId { get; set; }
        [JsonProperty("owner")]
        public Guid OwnerId { get; set; }
        [JsonProperty("comment")]
        public string CommentText { get; set; }
    }
}
