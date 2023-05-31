using AbobusMobile.Database.Services.Abstractions.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Models
{
    [Table("Resources")]
    public class ResourceModel : BaseModel
    {
        [Column("GlobalID"), Unique]
        public Guid GlobalId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Path"), Unique]
        public string Path { get; set; }
    }
}
