using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Requests.Locations
{
    public class GetLocationByIdRequest : AuthorizationRequest
    {
        private Guid locationId;

        protected override string Url => $"locations/{locationId}";

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public override Task<BaseResponse> SendRequestAsync()
        {
            locationId.ValidateNotEmpty();

            return base.SendRequestAsync();
        }

        public void Initialize(Guid desiredLocationId)
        {
            locationId = desiredLocationId;
        }
    }
}
