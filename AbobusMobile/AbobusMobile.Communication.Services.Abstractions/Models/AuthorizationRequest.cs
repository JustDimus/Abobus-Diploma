using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Communication.Services.Abstractions.Models
{
    public abstract class AuthorizationRequest : HttpRequest
    {
        public string AuthorizationToken { get; set; }

        public string AuthorizationHeaderName { get; set; }

        protected override void Configure()
        {
            base.Configure();

            if (AuthorizationToken != null && AuthorizationHeaderName != null)
            {
                Headers.Add(AuthorizationHeaderName, AuthorizationToken);
            }
        }
    }
}
