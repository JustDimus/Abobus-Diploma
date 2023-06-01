using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.DAL.Services.Abstractions.Monuments
{
    public class MonumentDataModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid CityId { get; set; }

        public string Description { get; set; }

        public Guid MonumentTitleImageId { get; set; }
    }
}
