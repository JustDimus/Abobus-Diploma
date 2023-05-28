using AbobusCore.Models.Session;
using AbobusMobile.Utilities.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Services.Abstractions.Models.Requests
{
    public class LoginRequest : AuthorizationRequest
    {
        protected override string Url => @"session/login";

        protected override HttpMethod HttpMethod => HttpMethod.Post;

        public override Task<BaseResponse> SendRequestAsync()
        {
            Content.ValidateIsNotNull();

            return base.SendRequestAsync();
        }

        public void Initialize(string login, string password)
        {
            Content = LoginCredentialsModel.Create(login, password);
        }
    }
}
