using AbobusMobile.Communication.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Services.Abstractions.Handlers
{
    public interface IAuthorizationHandler
    {
        event Func<BaseRequest, Task> OnAuthorizationFailed;
    }
}
