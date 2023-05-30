using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Communication.Services.Abstractions.Models
{
    public class HttpResponse : BaseResponse
    {
        public override bool Succeeded => statusCode == 200;

        public HttpResponse(int statusCode, string responseBody)
        {
            base.statusCode = statusCode;
            base.responseContent = responseBody;
        }

        public HttpResponse(int statusCode)
        {
            base.statusCode = statusCode;
        }
    }
}
