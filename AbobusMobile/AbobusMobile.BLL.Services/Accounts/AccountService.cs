using AbobusCore.Models.Accounts;
using AbobusCore.Models.Session;
using AbobusMobile.BLL.Services.Abstractions.Accounts;
using AbobusMobile.BLL.Services.Abstractions.Error;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.Communication.Requests.Accounts;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.DAL.Services.Abstractions.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Accounts
{
    public class AccountsService : IAccountsService
    {
        private IRequestFactory _requestFactory;
        private IAccountDataManager _accountDataManager;
        private IResourcesService _resourceService;
        private IErrorHandlingService _errorService;

        private bool dataSynchronized = false;

        private GetCurrentAccountDetailsRequest detailsRequest;
        private GetAccountStatisticsRequest statisticsRequest;

        public AccountsService(
            IAccountDataManager accountDataManager,
            IRequestFactory requestFactory,
            IResourcesService resourceService,
            IErrorHandlingService errorService)
        {
            _accountDataManager = accountDataManager ?? throw new ArgumentNullException(nameof(accountDataManager));
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
            _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
            _errorService = errorService ?? throw new ArgumentNullException(nameof(errorService));
        }

        private GetCurrentAccountDetailsRequest DetailsRequest => detailsRequest ?? (detailsRequest = _requestFactory.CreateRequest<GetCurrentAccountDetailsRequest>());
        private GetAccountStatisticsRequest StatisticsRequest => statisticsRequest ?? (statisticsRequest = _requestFactory.CreateRequest<GetAccountStatisticsRequest>());

        public async Task<AccountDetailsServiceModel> LoadAccountDetailsAsync()
        {
            await SynchronizeUserData();

            var accountDetailsData = await _accountDataManager.GetAccountDetailsAsync();

            return new AccountDetailsServiceModel()
            {
                Email = accountDetailsData.Email,
                Username = accountDetailsData.Username
            };
        }

        public async Task<Guid> GetCurrentAccountIdAsync()
        {
            await SynchronizeUserData();

            var accountDetailsData = await _accountDataManager.GetAccountDetailsAsync();

            return accountDetailsData.Id;
        }

        public async Task<Stream> LoadAccountImageAsync()
        {
            await SynchronizeUserData();

            var accountDetailsData = await _accountDataManager.GetAccountDetailsAsync();

            var profileImage = await _resourceService.GetResourceAsync(accountDetailsData.ProfilePhotoId);

            return profileImage.Resource;
        }

        public async Task<AccountStatisticsServiceModel> LoadAccountStatisticsAsync()
        {
            await SynchronizeUserData();

            var accountStatisticsData = await _accountDataManager.GetAccountStatisticsAsync();

            return new AccountStatisticsServiceModel()
            {
                DistanceUnit = accountStatisticsData.DistanceUnit,
                PassedDistance = accountStatisticsData.PassedDistance,
                FriendsCount = accountStatisticsData.FriendsCount,
                RoutesCount = accountStatisticsData.RoutesCount,
                VisitedCitiesCount = accountStatisticsData.VisitedCitiesCount
            };
        }

        private async Task SynchronizeUserData()
        {
            if (dataSynchronized)
            {
                return;
            }

            var detailsResult = await DetailsRequest.SendRequestAsync();

            if (!detailsResult.Succeeded)
            {
                // Implement error handling
                return;
            }

            var detailsResponse = detailsResult.As<AccountDetailsModel>();

            var newAccountData = new AccountDetailsDataModel()
            {
                Id = detailsResponse.Id,
                Email = detailsResponse.Email,
                Username = detailsResponse.Username,
                ProfilePhotoId = detailsResponse.ProfilePhotoId
            };

            await _accountDataManager.UpdateAccountDetailsAsync(newAccountData);

            await UpdateProfilePhotoImage();

            StatisticsRequest.Initialize(newAccountData.Id);

            var statisticsResult = await StatisticsRequest.SendRequestAsync();

            if (!statisticsResult.Succeeded)
            {
                // Implement error handling
                return;
            }

            var statisticsResponse = statisticsResult.As<AccountStatisticsModel>();

            var newStatisticsData = new AccountStatisticsDataModel()
            {
                DistanceUnit = statisticsResponse.DistanceUnit,
                PassedDistance = statisticsResponse.PassedDistance,
                FriendsCount = statisticsResponse.FriendsCount,
                RoutesCount = statisticsResponse.RoutesCount,
                VisitedCitiesCount = statisticsResponse.VisitedCitiesCount,
            };

            await _accountDataManager.UpdateAccountStatisticsAsync(newStatisticsData);

            dataSynchronized = true;
        }

        private async Task UpdateProfilePhotoImage()
        {
            var accountDetails = await _accountDataManager.GetAccountDetailsAsync();
            var resourceStatus = await _resourceService.GetResourceStatusAsync(accountDetails.ProfilePhotoId);

            switch (resourceStatus)
            {
                case ResourceServiceStatus.Available:
                    var result = await _resourceService.DownloadResourceAsync(accountDetails.ProfilePhotoId);
                    if (result != ResourceServiceStatus.Downloaded)
                    {
                        ThrowInvalidResourceException(accountDetails.ProfilePhotoId);
                    }
                    break;
                case ResourceServiceStatus.Unknown:
                case ResourceServiceStatus.NotFound:
                    ThrowInvalidResourceException(accountDetails.ProfilePhotoId);
                    break;
            }
        }

        private void ThrowInvalidResourceException(Guid resourceId)
        {
            throw new InvalidOperationException($"Could not download resource {resourceId}");
        }
    }
}
