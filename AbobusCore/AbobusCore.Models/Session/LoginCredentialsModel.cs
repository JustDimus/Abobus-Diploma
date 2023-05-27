using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusCore.Models.Session
{
    public class LoginCredentialsModel
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public static LoginCredentialsModel Create(string login, string password)
            => new LoginCredentialsModel { Login = login, Password = password };
    }
}
