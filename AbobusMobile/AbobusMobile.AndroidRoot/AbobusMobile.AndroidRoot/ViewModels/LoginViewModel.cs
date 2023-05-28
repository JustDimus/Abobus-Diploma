using AbobusMobile.AndroidRoot.Views;
using AbobusMobile.BLL.Services.Abstractions.Authorization;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthorizationService _authorizationService;

        public LoginViewModel(
            IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));

            LoginCommand = new Command(async () => await OnLoginClicked(), () => ValidateLoginProperties());

            PropertyChanged += (_, __) => LoginCommand.ChangeCanExecute();
        }

        public Command LoginCommand { get; }

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
            var loginModel = new LoginAuthorizationModel()
            {
                Email = Email,
                Password = Password,
            };

            var result = await _authorizationService.LoginAsync(loginModel);

            if (result == AuthorizationStatus.Authorized)
            {
                await Shell.Current.GoToAsync("//main");
            }
        }

        private bool ValidateLoginProperties()
            => Email.IsEmail()
            && Password.IsNotNullOrWhiteSpace();
    }
}
