using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.BLL.Services.Abstractions.Monuments
{
    public class MonumentServiceModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid CityId { get; set; }

        public string Description { get; set; }

        public Guid MonumentTitleImageId { get; set; }
    }
}
