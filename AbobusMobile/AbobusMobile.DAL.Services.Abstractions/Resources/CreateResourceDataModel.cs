using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AbobusMobile.DAL.Services.Abstractions.Resources
{
    public class CreateResourceDataModel
    {
        public Guid GlobalId { get; set; }

        public string Name { get; set; }

        public Stream SourceStream { get; set; }
    }
}
