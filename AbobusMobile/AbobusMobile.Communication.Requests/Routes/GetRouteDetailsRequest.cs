using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Requests.Routes
{
    public class GetRouteDetailsRequest : AuthorizationRequest
    {
        private Guid routeId;

        protected override string Url => $"routes/{routeId}/details";

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public void Initialize(Guid desiredRouteId)
        {
            routeId = desiredRouteId;
        }

        public override Task<BaseResponse> SendRequestAsync()
        {
            routeId.ValidateNotEmpty();

            return base.SendRequestAsync();
        }
    }
}
