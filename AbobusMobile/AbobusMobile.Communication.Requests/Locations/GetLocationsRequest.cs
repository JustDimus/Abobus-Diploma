using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Requests.Locations
{
    public class GetLocationsRequest : AuthorizationRequest
    {
        private string cityNamePattern;

        protected override string Url => $"locations?cityname={cityNamePattern}";

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public override Task<BaseResponse> SendRequestAsync()
        {
            cityNamePattern.ValidateIsNotNull();

            return base.SendRequestAsync();
        }

        public void Initialize(string desiredCityNamePattern)
        {
            cityNamePattern = desiredCityNamePattern;
        }
    }
}
