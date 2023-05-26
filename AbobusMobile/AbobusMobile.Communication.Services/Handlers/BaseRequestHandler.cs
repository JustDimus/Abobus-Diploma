using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Communication.Services.Handlers
{
    public abstract class BaseRequestHandler<TRequest> : IRequestHandler<TRequest> where TRequest : BaseRequest, new()
    {
        public abstract TRequest HandleRequest(TRequest request);

        public BaseRequest HandleRequest(BaseRequest request)
        {
            if (request is TRequest exactRequest)
            { 
                return HandleRequest(exactRequest);
            }

            return request;
        }

        public abstract TRequest HandleResponse(TRequest request);

        public BaseRequest HandleResponse(BaseRequest request)
        {
            if (request is TRequest exactResponse)
            {
                return HandleResponse(exactResponse);
            }

            return request;
        }
    }
}
