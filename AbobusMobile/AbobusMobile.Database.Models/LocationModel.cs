using AbobusMobile.Database.Services.Abstractions.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Models
{
    [Table("Locations")]
    public class LocationModel : BaseModel
    {
        [Column("CityID"), Unique]
        public Guid CityId { get; set; }

        [Column("Name")]
        public string CityName { get; set; }
    }
}
