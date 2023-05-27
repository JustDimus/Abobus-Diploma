using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Communication.Services.Abstractions.Configuration
{
    public class RequestConsumerServiceConfiguration
    {
        public bool UseRelativeUrls { get; set; }

        public string BaseURL { get; set; }
    }
}
