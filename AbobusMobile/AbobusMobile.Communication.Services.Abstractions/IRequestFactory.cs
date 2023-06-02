using AbobusMobile.Communication.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Communication.Services.Abstractions
{
    public interface IRequestFactory
    {
        TRequest CreateRequest<TRequest>() where TRequest : BaseRequest, new();

        BaseRequest CreateRequest(Type requestType);
    }
}
