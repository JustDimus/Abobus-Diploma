using AbobusMobile.Database.Services.Abstractions.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Models
{
    [Table("MonumentImages")]
    public class MonumentImageModel : BaseModel
    {
        [Column("MonumentID")]
        public Guid MonumentId { get; set; }
        [Column("ImageID")]
        public Guid ImageId { get; set; }
    }
}
