using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Session
{
    public class LoginCredentialsModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public static LoginCredentialsModel Create(string email, string password)
            => new LoginCredentialsModel { Email = email, Password = password };
    }
}
