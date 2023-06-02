using AbobusMobile.Communication.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AbobusMobile.Communication.Services.Abstractions.Extensions
{
    public static class BaseResponseExtensions
    {
        public static bool NotFound(this BaseResponse response)
            => response.StatusCode == (int)HttpStatusCode.NotFound;
    }
}
