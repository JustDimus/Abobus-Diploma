using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Abstractions.Account
{
    public interface IAccountDataManager
    {
        Task<AccountDetailsDataModel> GetAccountDetailsAsync();

        Task<AccountStatisticsDataModel> GetAccountStatisticsAsync();

        Task UpdateAccountDetailsAsync(AccountDetailsDataModel accountDetails);

        Task UpdateAccountStatisticsAsync(AccountStatisticsDataModel accountStatistics);

        Task ClearAccountDataAsync();

        Task<bool> CheckAccountStatiscticsAvailabilityAsync();

        Task<bool> CheckAccountDetailsAvailabilityAsync();
    }
}
