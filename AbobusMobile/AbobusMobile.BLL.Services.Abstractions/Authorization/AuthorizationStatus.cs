using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.BLL.Services.Abstractions.Authorization
{
    public enum AuthorizationStatus
    {
        Unauthorized,
        Authorized,
        AuthorizationTokenExpired,
        OperationFailed,
        Unknown,
    }
}
