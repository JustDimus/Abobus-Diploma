using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Abstractions.Account
{
    public interface IAccountService
    {
        Task<Guid> GetCurrentAccountIdAsync();

        Task<AccountDetailsServiceModel> LoadAccountDetailsAsync();

        Task<Stream> LoadAccountImageAsync();

        Task<AccountStatisticsServiceModel> LoadAccountStatisticsAsync();
    }
}
