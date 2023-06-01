using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Accounts
{
    public class AccountPublicInfoModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("profile_photo")]
        public Guid ProfilePhotoId { get; set; }
    }
}
