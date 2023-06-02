using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Abstractions.Authorization
{
    public interface IAuthorizationService
    {
        IObservable<bool> AuthorizationNeededObservable { get; }

        Task<AuthorizationServiceStatus> CheckAuthorizationStatusAsync();

        Task<AuthorizationServiceStatus> RefreshAuthorizationAsync();

        Task<AuthorizationServiceStatus> LoginAsync(LoginAuthorizationServiceModel loginData);

        Task<AuthorizationServiceStatus> LogoutAsync();
    }
}
