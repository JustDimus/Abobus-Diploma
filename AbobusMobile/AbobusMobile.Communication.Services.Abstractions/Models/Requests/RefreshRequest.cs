using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AbobusMobile.Communication.Services.Abstractions.Models.Requests
{
    public class RefreshRequest : AuthorizationRequest
    {
        protected override string Url => @"session/refresh";

        protected override HttpMethod HttpMethod => HttpMethod.Post;
    }
}
