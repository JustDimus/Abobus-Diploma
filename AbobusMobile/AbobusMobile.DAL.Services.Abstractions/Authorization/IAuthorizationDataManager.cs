using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Abstractions.Authorization
{
    public interface IAuthorizationDataManager
    {
        Task<bool> CheckAuthorizationDataAvailabilityAsync();

        Task<AuthorizationDataModel> GetAuthorizationDataAsync();

        Task SetAuthorizationData(AuthorizationDataModel authorizationData);

        Task ClearAuthorizationDataAsync();
    }
}
