using AbobusMobile.Database.Services.Abstractions.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Models
{
    [Table("Routes")]
    public class RouteModel : BaseModel
    {
        [Column("RouteID"), Unique]
        public Guid RouteId { get; set; }
        [Column("CityID")]
        public Guid CityId { get; set; }
        [Column("CreatorID")]
        public Guid CreatorId { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Distance")]
        public int Distance { get; set; }
        [Column("DistanceUnit")]
        public string DistanceUnit { get; set; }
        [Column("ResourceID")]
        public Guid RouteResourceId { get; set; }
        [Column("ImageID")]
        public Guid RouteImageId { get; set; }
    }
}
