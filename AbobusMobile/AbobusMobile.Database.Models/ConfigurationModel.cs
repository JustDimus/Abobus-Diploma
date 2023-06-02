using AbobusMobile.Database.Services.Abstractions.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Models
{
    [Table("Configurations")]
    public class ConfigurationModel : BaseModel
    {
        [Column("Name"), Unique]
        public string Name { get; set; }

        [Column("Value")]
        public string Value { get; set; }
    }
}
