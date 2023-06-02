using AbobusMobile.Database.Services.Abstractions.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Models
{
    [Table("AccountsPublicInfo")]
    public class AccountPublicInfoModel : BaseModel
    {
        [Column("AccountID"), Unique]
        public Guid AccountId { get; set; }

        [Column("Username")]
        public string Username { get; set; }

        [Column("ProfilePhotoID")]
        public Guid ProfilePhotoId { get; set; }
    }
}
