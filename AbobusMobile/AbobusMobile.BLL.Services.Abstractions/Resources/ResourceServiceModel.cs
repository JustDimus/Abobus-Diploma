using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AbobusMobile.BLL.Services.Abstractions.Resources
{
    public class ResourceServiceModel
    {
        public string Name { get; set; }
        public MemoryStream Resource { get; set; }
    }
}
