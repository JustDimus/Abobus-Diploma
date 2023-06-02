using AbobusMobile.AndroidRoot.Helpers;
using AbobusMobile.AndroidRoot.Views;
using AbobusMobile.BLL.Services.Abstractions.Accounts;
using AbobusMobile.BLL.Services.Abstractions.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private IAuthorizationService _authorizationService;
        private IAccountsService _accountService;

        public ProfileViewModel(
            IAuthorizationService authorizationService,
            IAccountsService accountService)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));

            LogoutCommand = new Command(async (forceExit) =>
            {
                await OnLogoutClicked(Convert.ToBoolean(forceExit));
            });
        }

        #region Comments
        public Command SettingsCommand { get; }
        public Command LogoutCommand { get; }
        #endregion

        #region Properties
        private ImageSource profilePhoto;
        public ImageSource ProfilePhoto
        {
            get => profilePhoto;
            set => SetProperty(ref profilePhoto, value);
        }

        private string username;
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private int? routesCount;
        public int? RoutesCount
        {
            get => routesCount;
            set => SetProperty(ref routesCount, value);
        }

        private int? citiesCount;
        public int? CitiesCount
        {
            get => citiesCount;
            set => SetProperty(ref citiesCount, value);
        }

        private int? friendsCount;
        public int? FriendsCount
        {
            get => friendsCount;
            set => SetProperty(ref friendsCount, value);
        }

        private int? passedDistance;
        public int? PassedDistance
        {
            get => passedDistance;
            set => SetProperty(ref passedDistance, value);
        }

        private string distanceUnit = "km";
        public string DistanceUnit
        {
            get => distanceUnit;
            set => SetProperty(ref distanceUnit, value);
        }

        private bool logoutInitiated = false;
        public bool LogoutInitiated
        {
            get => logoutInitiated;
            set => SetProperty(ref logoutInitiated, value);
        }

        private bool profileUpdateRequired = true;

        public bool ProfileUpdateRequired
        {
            get => profileUpdateRequired;
            set => SetProperty(ref profileUpdateRequired, value);
        }
        #endregion

        protected override async void OnPageAppeared()
        {
            LogoutInitiated = false;

            if (ProfileUpdateRequired)
            {
                await UpdateProfile();
            }
        }

        private async Task UpdateProfile()
        {
            ProfileUpdateRequired = true;

            var accountDetails = await _accountService.LoadAccountDetailsAsync();
            Username = accountDetails.Username;
            Email = accountDetails.Email;

            var accountStatistics = await _accountService.LoadAccountStatisticsAsync();
            RoutesCount = accountStatistics.RoutesCount;
            CitiesCount = accountStatistics.VisitedCitiesCount;
            FriendsCount = accountStatistics.FriendsCount;
            PassedDistance = accountStatistics.PassedDistance;
            DistanceUnit = accountStatistics.DistanceUnit;

            await UpdateProfilePhoto();

            ProfileUpdateRequired = false;
        }

        private async Task UpdateProfilePhoto()
        {
            var profileImageStream = await _accountService.LoadAccountImageAsync();

            var profileImageSource = ImageHelper.Create(profileImageStream);

            ProfilePhoto = profileImageSource;
        }

        private async Task OnLogoutClicked(bool forceExit)
        {
            if (forceExit
                || LogoutInitiated)
            {
                var logoutResult = await _authorizationService.LogoutAsync();

                if (logoutResult == AuthorizationServiceStatus.Unauthorized)
                {
                    await Shell.Current.GoToAsync(PathConstants.LOGIN);
                }
                else
                {
                    // Handle error and go to error page
                }
            }
            else
            {
                LogoutInitiated = true;
            }
        }
    }
}
