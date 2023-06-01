using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Requests.Accounts
{
    public class GetAccountPublicInfoRequest : AuthorizationRequest
    {
        private Guid accountId;

        protected override string Url => $"accounts/{accountId}";

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public override Task<BaseResponse> SendRequestAsync()
        {
            accountId.ValidateNotEmpty();

            return base.SendRequestAsync();
        }

        public void Initialize(Guid requestedAccountId)
        {
            accountId = requestedAccountId;
        }
    }
}
