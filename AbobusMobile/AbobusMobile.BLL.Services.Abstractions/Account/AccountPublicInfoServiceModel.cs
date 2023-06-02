using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.BLL.Services.Abstractions.Accounts
{
    public class AccountPublicInfoServiceModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public Guid ProfilePhotoId { get; set; }
    }
}
