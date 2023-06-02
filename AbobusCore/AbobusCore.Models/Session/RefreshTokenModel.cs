using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Session
{
    public class RefreshTokenModel
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public static RefreshTokenModel Create(string refreshToken)
            => new RefreshTokenModel { RefreshToken = refreshToken };
    }
}
