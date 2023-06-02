using AbobusMobile.Utilities.Extensions;
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

        private Dictionary<string, string> requestHeaders;
        private bool requestDataExist;
        private string requestData;
        private string requestDataType;
        private string requestAddress;
        private object requestType;

        public IReadOnlyDictionary<string, string> RequestHeaders => requestHeaders;

        protected Dictionary<string, string> Headers => requestHeaders;

        public string RequestAddress
        {
            get => requestAddress;
            protected set
            {
                requestAddress.ValidateIsNull();
                requestAddress = value;
            }
        }

        public string RequestData
        {
            get => requestData;
            protected set
            {
                requestData.ValidateIsNull();
                requestData = value;
            }
        }

        public object RequestType
        {
            get => requestType;
            protected set
            {
                requestType.ValidateIsNull();
                requestType = value;
            }
        }

        public bool RequestDataExist
        {
            get => requestDataExist;
            protected set => requestDataExist = value;
        }

        public string RequestDataType
        {
            get => requestDataType;
            protected set
            {
                requestDataType.ValidateIsNull();
                requestDataType = value;
            }
        }

        public T GetRequestType<T>()
        {
            return (T)requestType;
        }

        public void ConfigureRequest() => Configure();

        protected virtual void Configure()
        {
            requestHeaders = new Dictionary<string, string>();
            requestData = null;
            requestAddress = null;
            requestType = null;
            requestDataType = null;
            requestDataExist = false;
        }

        public BaseResponse Response => response;

        public virtual Task<BaseResponse> SendRequestAsync()
        {
            return requestSender.SendRequest(this);
        }

        protected void SetRequestResponse(BaseResponse newResponse)
        {
            response = newResponse ?? throw new ArgumentNullException(nameof(newResponse));
        }

        protected void SetRequestFactory(IRequestSender requestSender)
        {
            this.requestSender = requestSender ?? throw new ArgumentNullException(nameof(requestSender));
        }
    }
}
