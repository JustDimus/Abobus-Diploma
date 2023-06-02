using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Requests.Routes
{
    public class GetRoutesDetailsRequest : AuthorizationRequest
    {
        private Guid cityId;

        protected override string Url => $"routes?city={cityId}";

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public override Task<BaseResponse> SendRequestAsync()
        {
            cityId.ValidateNotEmpty();

            return base.SendRequestAsync();
        }

        public void Initialize(Guid requestedCityId)
        {
            cityId = requestedCityId;
        }
    }
}
