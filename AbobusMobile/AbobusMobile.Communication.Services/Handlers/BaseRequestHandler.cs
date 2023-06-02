using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Services.Handlers
{
    public abstract class BaseRequestHandler<TRequest> : IRequestHandler<TRequest> where TRequest : BaseRequest
    {
        protected IRequestFactory RequestFactory { get; private set; }

        public abstract Task<TRequest> HandleRequest(TRequest request);

        public async Task<BaseRequest> HandleRequest(BaseRequest request)
        {
            if (request is TRequest exactRequest)
            { 
                return await HandleRequest(exactRequest);
            }

            return request;
        }

        public abstract Task<TRequest> HandleResponse(TRequest request);

        public async Task<BaseRequest> HandleResponse(BaseRequest request)
        {
            if (request is TRequest exactResponse)
            {
                return await HandleResponse(exactResponse);
            }

            return request;
        }

        public void SetupRequestFactory(IRequestFactory requestFactory)
        {
            RequestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
        }
    }
}
