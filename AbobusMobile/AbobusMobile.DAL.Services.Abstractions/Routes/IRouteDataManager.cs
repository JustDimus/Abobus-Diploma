using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Abstractions.Routes
{
    public interface IRoutesDataManager
    {
        Task<bool> CheckRouteAvailabilityAsync(Guid routeId);

        Task<RouteDataModel> GetAsync(Guid routeId);

        Task<List<RouteDataModel>> GetAllAsync(Guid cityId);

        Task DeleteAsync(Guid routeId);

        Task CreateAsync(RouteDataModel route);

        Task UpdateAsync(RouteDataModel route);
    }
}
