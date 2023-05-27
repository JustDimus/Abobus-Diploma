using AbobusCore.Models.Session;
using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Communication.Services.Abstractions.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Services.Handlers
{
    public class AuthorizatonHandler : BaseRequestHandler<AuthorizationRequest>
    {
        // Add database conenction
        public AuthorizatonHandler()
        {
            
        }

        public override Task<AuthorizationRequest> HandleRequest(AuthorizationRequest request)
        {
            // Setup authorization header
            throw new NotImplementedException();
        }

        public override async Task<AuthorizationRequest> HandleResponse(AuthorizationRequest request)
        {
            if (request.Response.StatusCode == CommunicationConstants.UNAUTHORIZED_STATUS_CODE)
            {
                // Check for Unauthorized 401 Error Code
            }

            if (request is LoginRequest loginRequest)
            {
                await AuthorizeApplication(loginRequest.Response.As<SessionResultModel>());
            }

            throw new NotImplementedException();
        }

        private async Task AuthorizeApplication(SessionResultModel sessionModel)
        {
            // TODO
        }
    }
}
