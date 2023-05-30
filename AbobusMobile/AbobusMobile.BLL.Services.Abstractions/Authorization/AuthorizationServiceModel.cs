using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.BLL.Services.Abstractions.Authorization
{
    public class AuthorizationServiceModel
    {
        public string AuthorizationToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpirationTime { get; set; }
    }
}
