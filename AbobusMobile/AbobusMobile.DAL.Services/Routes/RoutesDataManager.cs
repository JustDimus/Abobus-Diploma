using AbobusMobile.DAL.Services.Abstractions.Routes;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Routes
{
    public class RoutesDataManager : IRoutesDataManager
    {
        private IRepository<RouteModel> _routes;

        public RoutesDataManager(
            IRepository<RouteModel> routes)
        {
            _routes = routes ?? throw new ArgumentNullException(nameof(routes));
        }

        public async Task<bool> CheckRouteAvailabilityAsync(Guid routeId)
        {
            return await _routes.AnyAsync(i => i.RouteId == routeId);
        }

        public async Task CreateAsync(RouteDataModel route)
        {
            var newRouteModel = ToDbModel(route);

            await _routes.InsertAsync(newRouteModel);
        }

        public async Task DeleteAsync(Guid routeId)
        {
            var existingRoute = await _routes.FirstOrDefaultAsync(i => i.RouteId == routeId);

            if (existingRoute != null)
            {
                await _routes.DeleteAsync(existingRoute);
            }
        }

        public async Task<List<RouteDataModel>> GetAllAsync(Guid cityId)
        {
            var routes = await _routes.SelectAsync(i => i.CityId == cityId);

            return routes.Select(ToDataModel).ToList();
        }

        public async Task<RouteDataModel> GetAsync(Guid routeId)
        {
            var existingRoute = await _routes.FirstOrDefaultAsync(i => i.RouteId == routeId);

            if (existingRoute != null)
            {
                return ToDataModel(existingRoute);
            }

            return null;
        }

        public async Task UpdateAsync(RouteDataModel route)
        {
            await DeleteAsync(route.Id);

            await CreateAsync(route);
        }

        private RouteDataModel ToDataModel(RouteModel route)
            => new RouteDataModel()
            {
                CityId = route.CityId,
                CreatorId = route.CreatorId,
                Distance = route.Distance,
                DistanceUnit = route.DistanceUnit,
                Name = route.Name,
                RouteImageId = route.RouteImageId,
                RouteResourceId = route.RouteResourceId,
            };

        private RouteModel ToDbModel(RouteDataModel route)
            => new RouteModel()
            {
                CityId = route.CityId,
                CreatorId = route.CreatorId,
                Distance = route.Distance,
                DistanceUnit = route.DistanceUnit,
                Name = route.Name,
                RouteId = route.Id,
                RouteImageId = route.RouteImageId,
                RouteResourceId = route.RouteResourceId,
            };
    }
}
