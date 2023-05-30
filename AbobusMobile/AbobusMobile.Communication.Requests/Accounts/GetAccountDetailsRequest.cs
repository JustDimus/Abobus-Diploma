using AbobusMobile.Communication.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AbobusMobile.Communication.Requests.Accounts
{
    public class GetCurrentAccountDetailsRequest : AuthorizationRequest
    {
        protected override string Url => "accounts/current";

        protected override HttpMethod HttpMethod => HttpMethod.Get;
    }
}
