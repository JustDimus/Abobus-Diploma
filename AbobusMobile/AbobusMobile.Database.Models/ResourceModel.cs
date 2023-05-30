using AbobusMobile.Database.Services.Abstractions.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Models
{
    [Table("resources")]
    public class ResourceModel : BaseModel
    {
        [Column("global_id"), Unique]
        public Guid GlobalId { get; set; }

        [Column("Name"), Unique]
        public string Name { get; set; }

        [Column("Path")]
        public string Path { get; set; }
    }
}
