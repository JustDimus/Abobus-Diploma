﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Services.Abstractions.Models
{
    public class BaseModel
    {
        [PrimaryKey, AutoIncrement]
        [Column("ID")]
        public int Id { get; set; }
    }
}
