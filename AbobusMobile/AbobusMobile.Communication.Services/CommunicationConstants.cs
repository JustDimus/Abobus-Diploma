using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Communication.Services
{
    public static class CommunicationConstants
    {
        public const string REQUEST_SENDER_SETUP_METHOD_NAME = "SetRequestFactory";
        public const string RESPONSE_SETUP_METHOD_NAME = "SetRequestResponse";

        public const int UNAUTHORIZED_STATUS_CODE = 401;
        public const int EXCEPTION_STATUS_CODE = -1;
    }
}
