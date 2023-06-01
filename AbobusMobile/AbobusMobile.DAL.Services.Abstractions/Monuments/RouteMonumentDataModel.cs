using System;
using System.Collections.Generic;

namespace AbobusMobile.DAL.Services.Abstractions.Monuments
{
    public class RouteMonumentsDataModel
    {
        public Guid RouteId { get; set; }
        public List<Guid> MonumentsId { get; set; }
    }
}