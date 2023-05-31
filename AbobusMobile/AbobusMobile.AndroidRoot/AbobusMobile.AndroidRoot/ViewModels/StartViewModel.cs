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
                case AuthorizationServiceStatus.AuthorizationTokenExpired:
                    var result = await RefreshAuthorization();

                    await Shell.Current.GoToAsync(
                        result == AuthorizationServiceStatus.Authorized
                        ? PathConstants.MAIN
                        : PathConstants.LOGIN);

                    break;
                case AuthorizationServiceStatus.Authorized:
                    await Shell.Current.GoToAsync(PathConstants.ROUTES_ABSOLUTE);
                    break;
                case AuthorizationServiceStatus.Unauthorized:
                    await Shell.Current.GoToAsync(PathConstants.LOGIN);
                    break;
                default:
                    throw new InvalidOperationException($"There is no corresponding switch-case statement for {currentAuthorizationStatus}");
            }
        }

        private async Task<AuthorizationServiceStatus> RefreshAuthorization()
            => await _authorizationService.RefreshAuthorizationAsync();
    }
}
