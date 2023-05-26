using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Services.Abstractions.Models
{
    public abstract class BaseRequest
    {
        private IRequestSender requestSender = null;

        private BaseResponse response = null;

        public BaseResponse Response => response;

        public Task<BaseResponse> SendRequest()
        {
            return requestSender.SendRequest(this);
        }

        private void SetRequestResponse(BaseResponse newResponse)
        {
            response = newResponse ?? throw new ArgumentNullException(nameof(newResponse));
        }

        private void SetRequestFactory(IRequestSender requestSender)
        {
            this.requestSender = requestSender ?? throw new ArgumentNullException(nameof(requestSender));
        }
    }
}
