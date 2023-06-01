using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Abstractions.Accounts
{
    public interface IAccountsDataManager
    {
        Task<AccountDetailsDataModel> GetCurrentAccountDetailsAsync();

        Task<AccountStatisticsDataModel> GetCurrentAccountStatisticsAsync();

        Task UpdateCurrentAccountDetailsAsync(AccountDetailsDataModel accountDetails);

        Task UpdateCurrentAccountStatisticsAsync(AccountStatisticsDataModel accountStatistics);

        Task ClearCurrentAccountDataAsync();

        Task<bool> CheckCurrentAccountStatiscticsAvailabilityAsync();

        Task<bool> CheckCurrentAccountDetailsAvailabilityAsync();

        Task<bool> CheckAccountPublicInfoAvailabilityAsync(Guid accountId);

        Task<AccountPublicInfoDataModel> GetAccountPublicInfoAsync(Guid accountId);

        Task CreateAccountPublicInfoAsync(AccountPublicInfoDataModel accountDataModel);

        Task DeleteAccountPublicInfoAsync(Guid accountId);
    }
}
