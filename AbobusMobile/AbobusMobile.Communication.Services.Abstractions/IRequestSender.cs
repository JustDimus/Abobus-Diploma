using AbobusMobile.Communication.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Services.Abstractions
{
    public interface IRequestSender
    {
        Task<BaseResponse> SendRequest(BaseRequest request);
    }
}
