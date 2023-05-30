using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Requests.Resources
{
    public class GetResourceInfoRequest : AuthorizationRequest
    {
        private Guid resourceId;

        protected override string Url => $"resources/{resourceId}";

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public void Initialize(Guid globalResourceId)
        {
            resourceId = globalResourceId;
        }

        public override Task<BaseResponse> SendRequestAsync()
        {
            resourceId.ValidateNotEmpty();

            return base.SendRequestAsync();
        }
    }
}
