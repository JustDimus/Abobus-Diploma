using AbobusMobile.Communication.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Services.Abstractions
{
    public interface IRequestHandler
    {
        Task<BaseRequest> HandleRequest(BaseRequest request);

        Task<BaseRequest> HandleResponse(BaseRequest request);

        void SetupRequestFactory(IRequestFactory requestFactory);
    }

    public interface IRequestHandler<TRequest> : IRequestHandler where TRequest : BaseRequest
    {
        Task<TRequest> HandleRequest(TRequest request);

        Task<TRequest> HandleResponse(TRequest request);
    }
}
