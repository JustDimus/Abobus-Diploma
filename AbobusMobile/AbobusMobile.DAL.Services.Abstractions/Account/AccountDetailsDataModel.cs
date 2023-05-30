using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.DAL.Services.Abstractions.Account
{
    public class AccountDetailsDataModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public Guid ProfilePhotoId { get; set; }
    }
}
