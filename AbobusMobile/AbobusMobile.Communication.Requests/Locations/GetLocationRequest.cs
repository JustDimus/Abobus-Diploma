using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Requests.Locations
{
    public class GetLocationRequest : AuthorizationRequest
    {
        private double longitude;
        private double latitude;

        protected override string Url => $"locations/current?longitude={longitude}&latitude={latitude}";

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public override Task<BaseResponse> SendRequestAsync()
        {
            longitude.ValidateIsNotEmpty();
            latitude.ValidateIsNotEmpty();

            return base.SendRequestAsync();
        }

        public void Initialize(double desiredLongitude, double desiredLatitude)
        {
            longitude = desiredLongitude;
            latitude = desiredLatitude;
        }
    }
}
