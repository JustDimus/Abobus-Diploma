using AbobusMobile.Database.Services.Abstractions.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Models
{
    [Table("Monuments")]
    public class MonumentModel : BaseModel
    {
        [Column("MonumentID"), Unique]
        public Guid MonumentId { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("CityID")]
        public Guid CityId { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("TitleImageID")]
        public Guid MonumentTitleImageId { get; set; }
    }
}
