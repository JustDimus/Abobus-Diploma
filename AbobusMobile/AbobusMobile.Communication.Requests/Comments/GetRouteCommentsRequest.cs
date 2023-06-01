using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Requests.Comments
{
    public class GetRouteCommentsRequest : AuthorizationRequest
    {
        private Guid routeId;

        protected override string Url => $"routes/{routeId}/comments";

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public override Task<BaseResponse> SendRequestAsync()
        {
            routeId.ValidateNotEmpty();

            return base.SendRequestAsync();
        }

        public void Initialize(Guid requestedRouteId)
        {
            routeId = requestedRouteId;
        }
    }
}
