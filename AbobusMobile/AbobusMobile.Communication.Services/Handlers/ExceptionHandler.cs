using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Services.Handlers
{
    public class ExceptionHandler : BaseRequestHandler<HttpRequest>
    {
        public override Task<HttpRequest> HandleRequest(HttpRequest request)
        {
            return Task.FromResult(request);
        }

        public override Task<HttpRequest> HandleResponse(HttpRequest request)
        {
            if (request.Response.StatusCode == CommunicationConstants.EXCEPTION_STATUS_CODE)
            {
                var exception = request.Response.Exception;
            }

            return Task.FromResult(request);
        }
    }
}
