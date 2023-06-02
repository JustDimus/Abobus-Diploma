using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.BLL.Services.Abstractions.Authorization
{
    public enum AuthorizationServiceStatus
    {
        Unauthorized,
        Authorized,
        AuthorizationTokenExpired,
        OperationFailed,
        Unknown,
    }
}
