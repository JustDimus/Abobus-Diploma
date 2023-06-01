using AbobusCore.Models.Accounts;
using AbobusCore.Models.Session;
using AbobusMobile.BLL.Services.Abstractions.Accounts;
using AbobusMobile.BLL.Services.Abstractions.Error;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.Communication.Requests.Accounts;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.DAL.Services.Abstractions.Accounts;
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
        private IAccountsDataManager _accountDataManager;
        private IResourcesService _resourceService;
        private IErrorHandlingService _errorService;

        private bool dataSynchronized = false;

        private GetCurrentAccountDetailsRequest detailsRequest;
        private GetAccountStatisticsRequest statisticsRequest;
        private GetAccountPublicInfoRequest publicInfoRequest;

        public AccountsService(
            IAccountsDataManager accountDataManager,
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
        private GetAccountPublicInfoRequest PublicInfoRequest => publicInfoRequest ?? (publicInfoRequest = _requestFactory.CreateRequest<GetAccountPublicInfoRequest>());

        public async Task<AccountDetailsServiceModel> LoadAccountDetailsAsync()
        {
            await SynchronizeUserData();

            var accountDetailsData = await _accountDataManager.GetCurrentAccountDetailsAsync();

            return new AccountDetailsServiceModel()
            {
                Email = accountDetailsData.Email,
                Username = accountDetailsData.Username
            };
        }

        public async Task<AccountPublicInfoServiceModel> LoadAccountInfo(Guid accountId)
        {
            AccountPublicInfoServiceModel result = null;

            var accountInfoDownloaded = await _accountDataManager.CheckAccountPublicInfoAvailabilityAsync(accountId);

            if (accountInfoDownloaded)
            {
                var accountDataModel = await _accountDataManager.GetAccountPublicInfoAsync(accountId);

                result = GetServiceModel(accountDataModel);
            }

            PublicInfoRequest.Initialize(accountId);

            var response = await PublicInfoRequest.SendRequestAsync();

            if (response.Succeeded)
            {
                var accountModel = response.As<AccountPublicInfoModel>();

                await _accountDataManager.CreateAccountPublicInfoAsync(GetDataModel(accountModel));

                result = GetServiceModel(accountModel);
            }

            return result;
        }

        public async Task<Guid> GetCurrentAccountIdAsync()
        {
            await SynchronizeUserData();

            var accountDetailsData = await _accountDataManager.GetCurrentAccountDetailsAsync();

            return accountDetailsData.Id;
        }

        public async Task<Stream> LoadAccountImageAsync()
        {
            await SynchronizeUserData();

            var accountDetailsData = await _accountDataManager.GetCurrentAccountDetailsAsync();

            var profileImage = await _resourceService.GetResourceAsync(accountDetailsData.ProfilePhotoId);

            return profileImage.Resource;
        }

        public async Task<AccountStatisticsServiceModel> LoadAccountStatisticsAsync()
        {
            await SynchronizeUserData();

            var accountStatisticsData = await _accountDataManager.GetCurrentAccountStatisticsAsync();

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

            await _accountDataManager.UpdateCurrentAccountDetailsAsync(newAccountData);

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

            await _accountDataManager.UpdateCurrentAccountStatisticsAsync(newStatisticsData);

            dataSynchronized = true;
        }

        private async Task UpdateProfilePhotoImage()
        {
            var accountDetails = await _accountDataManager.GetCurrentAccountDetailsAsync();
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

        private AccountPublicInfoServiceModel GetServiceModel(AccountPublicInfoModel infoModel)
            => new AccountPublicInfoServiceModel()
            {
                Id = infoModel.Id,
                ProfilePhotoId = infoModel.ProfilePhotoId,
                Username = infoModel.Username,
            };
        private AccountPublicInfoServiceModel GetServiceModel(AccountPublicInfoDataModel infoModel)
            => new AccountPublicInfoServiceModel()
            {
                Id = infoModel.Id,
                ProfilePhotoId = infoModel.ProfilePhotoId,
                Username = infoModel.Username,
            };

        private AccountPublicInfoDataModel GetDataModel(AccountPublicInfoModel infoModel)
            => new AccountPublicInfoDataModel()
            {
                Id = infoModel.Id,
                ProfilePhotoId = infoModel.ProfilePhotoId,
                Username = infoModel.Username,
            };
    }
}
