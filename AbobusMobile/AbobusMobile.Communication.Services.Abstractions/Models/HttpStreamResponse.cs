using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AbobusMobile.Communication.Services.Abstractions.Models
{
    public class HttpStreamResponse : HttpResponse
    {
        public HttpStreamResponse(
            int statusCode, Stream responseStream)
            : base(statusCode)
        {
            base.responseContent = responseStream;
        }
    }
}
