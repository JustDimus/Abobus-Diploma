using AbobusMobile.DAL.Services.Abstractions.Accounts;
using AbobusMobile.DAL.Services.Abstractions.Configurations;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.Abstractions;
using AbobusMobile.Utilities.Exceptions;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Accounts
{
    public class AccountsDataManager : IAccountsDataManager
    {
        private readonly IConfigurationsDataManager _configurationManager;
        private readonly IRepository<AccountPublicInfoModel> _accounts;

        public AccountsDataManager(
            IConfigurationsDataManager configurationManager,
            IRepository<AccountPublicInfoModel> accountsRepository)
        {
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
            _accounts = accountsRepository ?? throw new ArgumentNullException(nameof(accountsRepository));
        }

        public async Task<bool> CheckAccountPublicInfoAvailabilityAsync(Guid accountId)
        {
            return await _accounts.AnyAsync(i => i.AccountId == accountId);
        }

        public async Task CreateAccountPublicInfoAsync(AccountPublicInfoDataModel accountDataModel)
        {
            var accountModel = await _accounts.FirstOrDefaultAsync(i => i.AccountId == accountDataModel.Id);

            if (accountModel != null)
            {
                accountModel.ProfilePhotoId = accountDataModel.ProfilePhotoId;
                accountModel.Username = accountDataModel.Username;

                await _accounts.UpdateAsync(accountModel);
            }
            else
            {
                await _accounts.InsertAsync(new AccountPublicInfoModel()
                {
                    AccountId = accountDataModel.Id,
                    ProfilePhotoId = accountDataModel.ProfilePhotoId,
                    Username = accountDataModel.Username,
                });
            }
        }

        public async Task<AccountPublicInfoDataModel> GetAccountPublicInfoAsync(Guid accountId)
        {
            var accountInfo = await _accounts.FirstOrDefaultAsync(i => i.AccountId == accountId);

            AccountPublicInfoDataModel result = null;

            if (accountInfo != null)
            {
                result = new AccountPublicInfoDataModel()
                {
                    Id = accountInfo.AccountId,
                    ProfilePhotoId = accountInfo.ProfilePhotoId,
                    Username = accountInfo.Username,
                };
            }

            return result;
        }

        public async Task DeleteAccountPublicInfoAsync(Guid accountId)
        {
            var accountPublicInfo = await _accounts.FirstOrDefaultAsync(i => i.AccountId == accountId);

            if (accountPublicInfo != null)
            {
                await _accounts.DeleteAsync(accountPublicInfo);
            }
        }

        public async Task ClearCurrentAccountDataAsync()
        {
            await ClearAccountConfiguration();
        }

        public async Task<AccountDetailsDataModel> GetCurrentAccountDetailsAsync()
        {
            var configurations = await SelectAccountDetailsConfigurations();

            AccountDetailsDataModel result = null;

            if (configurations.Count == AccountDataConstants.DETAILS_CONFIG_COUNT)
            {
                result = new AccountDetailsDataModel()
                {
                    Email = configurations.GetString(AccountDataConstants.DETAILS_EMAIL),
                    Username = configurations.GetString(AccountDataConstants.DETAILS_USERNAME),
                    ProfilePhotoId = configurations.GetGuid(AccountDataConstants.DETAILS_PROFILE_PHOTO_NAME),
                    Id = configurations.GetGuid(AccountDataConstants.DETAILS_ID),
                };
            }

            return result;
        }

        public async Task<AccountStatisticsDataModel> GetCurrentAccountStatisticsAsync()
        {
            var configurations = await SelectAccountStatiscticsConfigurations();

            AccountStatisticsDataModel result = null;

            if (configurations.Count == AccountDataConstants.STATISTICS_CONFIG_COUNT)
            {
                result = new AccountStatisticsDataModel()
                {
                    DistanceUnit = configurations.GetString(AccountDataConstants.STATISTICS_DISTANCE_UNIT),
                    FriendsCount = configurations.GetInt(AccountDataConstants.STATISTICS_FRIENDS),
                    PassedDistance = configurations.GetInt(AccountDataConstants.STATISTICS_DISTANCE),
                    RoutesCount = configurations.GetInt(AccountDataConstants.STATISTICS_ROUTES),
                    VisitedCitiesCount = configurations.GetInt(AccountDataConstants.STATISTICS_CITIES)
                };
            }

            return result;
        }

        public async Task UpdateCurrentAccountDetailsAsync(AccountDetailsDataModel details)
        {
            ValidateModel(details);

            await UpdateConfiguration(
                AccountDataConstants.DETAILS_ID, details.Id.ToString());

            await UpdateConfiguration(
                AccountDataConstants.DETAILS_USERNAME, details.Username);

            await UpdateConfiguration(
                AccountDataConstants.DETAILS_EMAIL, details.Email);

            await UpdateConfiguration(
                AccountDataConstants.DETAILS_PROFILE_PHOTO_NAME, details.ProfilePhotoId.ToString());
        }

        public async Task UpdateCurrentAccountStatisticsAsync(AccountStatisticsDataModel statistics)
        {
            ValidateModel(statistics);

            await UpdateConfiguration(
                AccountDataConstants.STATISTICS_CITIES, statistics.VisitedCitiesCount.ToString());

            await UpdateConfiguration(
                AccountDataConstants.STATISTICS_DISTANCE, statistics.PassedDistance.ToString());

            await UpdateConfiguration(
                AccountDataConstants.STATISTICS_DISTANCE_UNIT, statistics.DistanceUnit);

            await UpdateConfiguration(
                AccountDataConstants.STATISTICS_FRIENDS, statistics.FriendsCount.ToString());

            await UpdateConfiguration(
                AccountDataConstants.STATISTICS_ROUTES, statistics.RoutesCount.ToString());
        }

        public async Task<bool> CheckCurrentAccountDetailsAvailabilityAsync()
        {
            var configurations = await SelectAccountDetailsConfigurations();

            return configurations.Count == AccountDataConstants.DETAILS_CONFIG_COUNT;
        }

        public async Task<bool> CheckCurrentAccountStatiscticsAvailabilityAsync()
        {
            var configurations = await SelectAccountStatiscticsConfigurations();

            return configurations.Count == AccountDataConstants.STATISTICS_CONFIG_COUNT;
        }

        private async Task<List<ConfigurationModel>> SelectAccountDetailsConfigurations()
            => await _configurationManager.SelectAsync(configuration
                => configuration.Name == AccountDataConstants.DETAILS_USERNAME
                || configuration.Name == AccountDataConstants.DETAILS_ID
                || configuration.Name == AccountDataConstants.DETAILS_EMAIL
                || configuration.Name == AccountDataConstants.DETAILS_PROFILE_PHOTO_NAME);

        private async Task<List<ConfigurationModel>> SelectAccountStatiscticsConfigurations()
            => await _configurationManager.SelectAsync(configuration
                => configuration.Name == AccountDataConstants.STATISTICS_CITIES
                || configuration.Name == AccountDataConstants.STATISTICS_DISTANCE
                || configuration.Name == AccountDataConstants.STATISTICS_ROUTES
                || configuration.Name == AccountDataConstants.STATISTICS_FRIENDS
                || configuration.Name == AccountDataConstants.STATISTICS_DISTANCE_UNIT);

        private void ValidateModel(AccountDetailsDataModel accountDetails)
        {
            if (accountDetails == null
                || !accountDetails.Email.IsNotNullOrWhiteSpace()
                || !accountDetails.Username.IsNotNullOrWhiteSpace()
                || !accountDetails.ProfilePhotoId.IsNotEmpty())
            {
                throw new ValidationException(nameof(accountDetails));
            }
        }

        private void ValidateModel(AccountStatisticsDataModel accountStatistics)
        {
            if (accountStatistics == null
                || !accountStatistics.DistanceUnit.IsNotNullOrWhiteSpace())
            {
                throw new ValidationException(nameof(accountStatistics));
            }
        }

        private async Task ClearAccountConfiguration()
        {
            var details = await SelectAccountDetailsConfigurations();
            var statistics = await SelectAccountStatiscticsConfigurations();

            foreach (var configuration in details.ContinueWith(statistics))
            {
                await _configurationManager.DeleteAsync(configuration.Name);
            }
        }

        private async Task UpdateConfiguration(string name, string value)
        {
            await _configurationManager.UpdateAsync(name, value);
        }
    }
}
