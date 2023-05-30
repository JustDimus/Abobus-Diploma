using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.BLL.Services.Abstractions.Authorization
{
    public class LoginAuthorizationServiceModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
