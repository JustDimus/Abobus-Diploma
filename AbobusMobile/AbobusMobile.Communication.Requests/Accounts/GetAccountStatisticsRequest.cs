using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Requests.Accounts
{
    public class GetAccountStatisticsRequest : AuthorizationRequest
    {
        private Guid accountId;

        protected override string Url => $"accounts/{accountId}/statistics";

        protected override HttpMethod HttpMethod => throw new NotImplementedException();

        public override Task<BaseResponse> SendRequestAsync()
        {
            accountId.ValidateNotEmpty();

            return base.SendRequestAsync();
        }

        public void Initialize(Guid requiredAccountId)
        {
            accountId = requiredAccountId;
        }
    }
}
