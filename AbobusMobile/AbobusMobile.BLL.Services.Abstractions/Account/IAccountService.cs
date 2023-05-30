using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Abstractions.Account
{
    public interface IAccountService
    {
        Task<AccountDetailsModel> LoadAccountDetailsAsync();

        Task<Stream> LoadAccountImageAsync();

        Task<AccountStatisticsModel> LoadAccountStatisticsAsync();
    }
}
