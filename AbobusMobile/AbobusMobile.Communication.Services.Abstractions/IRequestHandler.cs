using AbobusMobile.Communication.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Communication.Services.Abstractions
{
    public interface IRequestHandler
    {
        BaseRequest HandleRequest(BaseRequest request);

        BaseRequest HandleResponse(BaseRequest request);
    }

    public interface IRequestHandler<TRequest> : IRequestHandler where TRequest : BaseRequest
    {
        TRequest HandleRequest(TRequest request);

        TRequest HandleResponse(TRequest request);
    }
}
