using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.DAL.Services.Abstractions.Authorization
{
    public class AuthorizationDataModel
    {
        public string AuthorizationToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpirationTime { get; set; }
    }
}
