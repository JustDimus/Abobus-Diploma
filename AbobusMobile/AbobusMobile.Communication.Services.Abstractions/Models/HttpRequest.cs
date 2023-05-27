using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AbobusMobile.Communication.Services.Abstractions.Models
{
    public abstract class HttpRequest : BaseRequest
    {
        protected abstract string Url { get; }

        protected abstract HttpMethod HttpMethod { get; }

        protected virtual object Content { get; set; }

        protected override void Configure()
        {
            base.Configure();

            RequestAddress = Url;
            RequestType = HttpMethod;
            RequestDataExist = Content != null;

            if (Content != null)
            {
                RequestData = JsonConvert.SerializeObject(Content);
                RequestDataType = "application/json";
            }
        }
    }
}
