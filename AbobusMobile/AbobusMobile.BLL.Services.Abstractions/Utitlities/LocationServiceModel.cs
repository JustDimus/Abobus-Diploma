using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.BLL.Services.Abstractions.Utilities
{
    public class LocationServiceModel
    {
        public bool LocationFound { get; set; }

        public Guid CityId { get; set; }

        public string CityName { get; set; }
    }
}
