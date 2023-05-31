using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.BLL.Services.Abstractions.Account
{
    public class AccountPublicInfoServiceModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public Guid ProfileImageId { get; set; }
    }
}
