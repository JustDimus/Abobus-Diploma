using AbobusMobile.AndroidRoot.Views;
using AbobusMobile.BLL.Services.Abstractions.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private IAuthorizationService _authorizationService;

        public ProfileViewModel(
            IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));

            LogoutCommand = new Command(async (forceExit) =>
            {
                await OnLogoutClicked(Convert.ToBoolean(forceExit));
            });
        }

        #region Properties

        private ImageSource profilePhoto = null;
        public ImageSource ProfilePhoto
        {
            get => profilePhoto;
            set => SetProperty(ref profilePhoto, value);
        }

        private string username = "turboTheBoar";
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        private string email = "helllodsad@gmail.com";
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private int routesCount = 15;
        public int RoutesCount
        {
            get => routesCount;
            set => SetProperty(ref routesCount, value);
        }

        private int citiesCount = 27;
        public int CitiesCount
        {
            get => citiesCount;
            set => SetProperty(ref citiesCount, value);
        }

        private int friendsCount = 146;
        public int FriendsCount
        {
            get => friendsCount;
            set => SetProperty(ref friendsCount, value);
        }

        private int passedDistance = 14;
        public int PassedDistance
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

        public Command SettingsCommand { get; }
        public Command LogoutCommand { get; }

        #endregion

        protected override void OnPageAppeared()
        {
            LogoutInitiated = false;
        }

        private async Task OnLogoutClicked(bool forceExit)
        {
            if (forceExit
                || LogoutInitiated)
            {
                var logoutResult = await _authorizationService.LogoutAsync();

                if (logoutResult == AuthorizationStatus.Unauthorized)
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
