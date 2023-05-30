using AbobusMobile.Communication.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AbobusMobile.Communication.Requests.Session
{
    public class LogoutRequest : AuthorizationRequest
    {
        protected override string Url => "session/logout";

        protected override HttpMethod HttpMethod => HttpMethod.Post;
    }
}
