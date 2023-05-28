using AbobusCore.Models.Session;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Services.Abstractions.Models.Requests
{
    public class RefreshRequest : AuthorizationRequest
    {
        protected override string Url => "session/refresh";

        protected override HttpMethod HttpMethod => HttpMethod.Post;

        public override Task<BaseResponse> SendRequestAsync()
        {
            Content.ValidateIsNotNull();

            return base.SendRequestAsync();
        }

        public void Initialize(string refreshToken)
        {
            Content = RefreshTokenModel.Create(refreshToken);
        }
    }
}
