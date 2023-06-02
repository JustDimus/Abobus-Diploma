using AbobusCore.Models.Session;
using AbobusMobile.Communication.Services.Abstractions.Handlers;
using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.DAL.Services.Abstractions.Authorization;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Services.Handlers
{
    public class AuthorizatonHandler : BaseRequestHandler<AuthorizationRequest>, IAuthorizationHandler
    {
        private IAuthorizationDataManager _authorizationManager;

        private bool authorizationFailed = false;

        public AuthorizatonHandler(
            IAuthorizationDataManager authorizationManager)
        {
            _authorizationManager = authorizationManager ?? throw new ArgumentNullException(nameof(authorizationManager));
        }

        public event Func<BaseRequest, Task> OnAuthorizationFailed;

        public override async Task<AuthorizationRequest> HandleRequest(AuthorizationRequest request)
        {
            var authorizationDataAvailable = await _authorizationManager.CheckAuthorizationDataAvailabilityAsync();

            if (authorizationDataAvailable)
            {
                var authorizationData = await _authorizationManager.GetAuthorizationDataAsync();

                request.AuthorizationHeaderName = "Authorization";
                request.AuthorizationToken = authorizationData.AuthorizationToken;
            }

            return request;
        }

        public override async Task<AuthorizationRequest> HandleResponse(AuthorizationRequest request)
        {
            if (request.Response.StatusCode == CommunicationConstants.UNAUTHORIZED_STATUS_CODE)
            {
                if (!authorizationFailed)
                {
                    authorizationFailed = true;

                    await OnAuthorizationFailed?.Invoke(request);

                    authorizationFailed = false;
                }
            }

            return request;
        }
    }
}
