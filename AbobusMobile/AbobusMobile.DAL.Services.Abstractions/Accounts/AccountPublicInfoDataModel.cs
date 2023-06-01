using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.DAL.Services.Abstractions.Accounts
{
    public class AccountPublicInfoDataModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public Guid ProfilePhotoId { get; set; }
    }
}
