using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AbobusMobile.BLL.Services.Abstractions.Routes
{
    public class RouteDetailsServiceModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid CityId { get; set; } 

        public bool Downloaded { get; set; }

        public int Distance { get; set; }

        public string DistanceUnit { get; set; }

        public Guid RouteResourceId { get; set; }

        public Guid CreatorId { get; set; }

        public Guid RouteImageId { get; set; }
    }
}
