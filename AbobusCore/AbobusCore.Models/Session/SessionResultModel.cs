using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Session
{
    public class SessionResultModel
    {
        [JsonProperty("authorization_token")]
        public string AuthorizationToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
