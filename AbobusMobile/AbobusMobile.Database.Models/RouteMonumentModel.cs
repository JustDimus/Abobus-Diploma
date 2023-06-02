using AbobusMobile.Database.Services.Abstractions.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Models
{
    [Table("RouteMonuments")]
    public class RouteMonumentModel : BaseModel
    {
        [Column("RouteID")]
        public Guid RouteId { get; set; }
        [Column("MonumentID")]
        public Guid MonumentId { get; set; }
    }
}
