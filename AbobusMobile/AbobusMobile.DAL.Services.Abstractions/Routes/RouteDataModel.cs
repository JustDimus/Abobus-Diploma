using System;

namespace AbobusMobile.DAL.Services.Abstractions.Routes
{
    public class RouteDataModel
    {
        public Guid Id { get; set; }

        public Guid CityId { get; set; }

        public Guid CreatorId { get; set; }

        public string Name { get; set; }

        public int Distance { get; set; }

        public string DistanceUnit { get; set; }

        public Guid RouteResourceId { get; set; }

        public Guid RouteImageId { get; set; }
    }
}