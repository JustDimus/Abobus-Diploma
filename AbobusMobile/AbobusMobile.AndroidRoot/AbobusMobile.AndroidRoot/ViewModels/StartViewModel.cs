using AbobusMobile.AndroidRoot.Views;
using AbobusMobile.BLL.Services.Abstractions.Authorization;
using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        private readonly IAuthorizationService _authorizationService;

        public StartViewModel(
            IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        protected override async void OnPageAppeared()
        {
            var currentAuthorizationStatus = await _authorizationService.CheckAuthorizationStatusAsync();

            switch (currentAuthorizationStatus)
            {
                case AuthorizationStatus.AuthorizationTokenExpired:
                    var result = await RefreshAuthorization();

                    await Shell.Current.GoToAsync(
                        result == AuthorizationStatus.Authorized
                        ? PathConstants.MAIN
                        : PathConstants.LOGIN);

                    break;
                case AuthorizationStatus.Authorized:
                    await Shell.Current.GoToAsync(PathConstants.PROFILE_ABSOLUTE);
                    break;
                case AuthorizationStatus.Unauthorized:
                    await Shell.Current.GoToAsync(PathConstants.LOGIN);
                    break;
                default:
                    throw new InvalidOperationException($"There is no corresponding switch-case statement for {currentAuthorizationStatus}");
            }
        }

        private async Task<AuthorizationStatus> RefreshAuthorization()
            => await _authorizationService.RefreshAuthorizationAsync();
    }
}
