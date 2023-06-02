using AbobusMobile.AndroidRoot.Configurations;
using AbobusMobile.AndroidRoot.Views;
using AbobusMobile.BLL.Services.Abstractions.Authorization;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly Options<EndpointConfigurations> _endpointConfigurations;

        public LoginViewModel(
            IAuthorizationService authorizationService,
            Options<EndpointConfigurations> endpointConfigurations)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _endpointConfigurations = endpointConfigurations ?? throw new ArgumentNullException(nameof(endpointConfigurations));

            LoginCommand = new Command(async () => await OnLoginClicked(), () => ValidateLoginProperties());
            RegisterCommand = new Command(async () => await OnRegisterClicked());

            PropertyChanged += (_, __) => LoginCommand.ChangeCanExecute();
        }

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        private bool loginFailed = false;
        public bool LoginFailed
        {
            get => loginFailed;
            set => SetProperty(ref loginFailed, value);
        }

        private bool loginStarted = false;
        public bool LoginStarted
        {
            get => loginStarted;
            set => SetProperty(ref loginStarted, value);
        }
        
        private string email = "hello@gmail.com";
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string password = "password";
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private async Task OnLoginClicked()
        {
            LoginStarted = true;
            LoginFailed = false;

            var loginModel = new LoginAuthorizationServiceModel()
            {
                Email = Email,
                Password = Password,
            };

            var result = await _authorizationService.LoginAsync(loginModel);

            if (result == AuthorizationServiceStatus.Authorized)
            {
                await Shell.Current.GoToAsync(PathConstants.MAIN);
            }
            else
            {
                LoginFailed = true;
            }

            LoginStarted = false;
        }

        private async Task OnRegisterClicked()
        {
            await Browser.OpenAsync(_endpointConfigurations.Value.RegisterUrl);
        }

        private bool ValidateLoginProperties()
            => Email.IsEmail()
            && Password.IsNotNullOrWhiteSpace()
            && !LoginStarted;
    }
}
