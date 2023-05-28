using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Abstractions.Authorization
{
    public interface IAuthorizationService
    {
        IObservable<bool> AuthorizationNeededObservable { get; }

        Task<AuthorizationStatus> CheckAuthorizationStatusAsync();

        Task<AuthorizationStatus> LoginAsync(LoginAuthorizationModel loginData);

        Task<AuthorizationStatus> LogoutAsync();
    }
}
