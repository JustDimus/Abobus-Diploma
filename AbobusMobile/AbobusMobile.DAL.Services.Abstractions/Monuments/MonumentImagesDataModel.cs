using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.DAL.Services.Abstractions.Monuments
{
    public class MonumentImagesDataModel
    {
        public Guid MonumentId { get; set; }

        public List<Guid> MonumentImagesId { get; set; }
    }
}
