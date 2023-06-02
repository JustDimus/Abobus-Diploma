using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Requests.Monuments
{
    public class GetMonumentImagesRequest : AuthorizationRequest
    {
        private Guid monumentId;

        protected override string Url => $"monuments/{monumentId}/images";

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public override Task<BaseResponse> SendRequestAsync()
        {
            monumentId.ValidateNotEmpty();

            return base.SendRequestAsync();
        }

        public void Initialize(Guid requestedMonumentId)
        {
            monumentId = requestedMonumentId;
        }
    }
}
