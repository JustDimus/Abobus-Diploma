using AbobusMobile.DAL.Services.Abstractions.Routes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Routes
{
    public class RoutesDataManager : IRoutesDataManager
    {
        public RoutesDataManager()
        {
            
        }

        public Task<bool> CheckRouteAvailabilityAsync(Guid routeId)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(RouteDataModel route)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid routeId)
        {
            throw new NotImplementedException();
        }

        public Task<List<RouteDataModel>> GetAllAsync(Guid cityId)
        {
            throw new NotImplementedException();
        }

        public Task<RouteDataModel> GetAsync(Guid routeId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(RouteDataModel route)
        {
            throw new NotImplementedException();
        }
    }
}
