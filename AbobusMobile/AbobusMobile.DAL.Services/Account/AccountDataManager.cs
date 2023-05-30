using AbobusMobile.DAL.Services.Abstractions.Account;
using AbobusMobile.DAL.Services.Abstractions.Configurations;
using AbobusMobile.Database.Models;
using AbobusMobile.Utilities.Exceptions;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Account
{
    public class AccountDataManager : IAccountDataManager
    {
        private readonly IConfigurationsDataManager _configurationManager;

        public AccountDataManager(IConfigurationsDataManager configurationManager)
        {
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
        }

        public async Task ClearAccountDataAsync()
        {
            await ClearAccountConfiguration();
        }

        public async Task<AccountDetailsDataModel> GetAccountDetailsAsync()
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

        public async Task<AccountStatisticsDataModel> GetAccountStatisticsAsync()
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

        public async Task UpdateAccountDetailsAsync(AccountDetailsDataModel details)
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

        public async Task UpdateAccountStatisticsAsync(AccountStatisticsDataModel statistics)
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

        public async Task<bool> CheckAccountDetailsAvailabilityAsync()
        {
            var configurations = await SelectAccountDetailsConfigurations();

            return configurations.Count == AccountDataConstants.DETAILS_CONFIG_COUNT;
        }

        public async Task<bool> CheckAccountStatiscticsAvailabilityAsync()
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
