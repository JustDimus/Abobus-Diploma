using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Abstractions.Monuments
{
    public interface IMonumentsDataManager
    {
        Task<bool> CheckMonumentAvailabilityAsync(Guid monumentId);

        Task<MonumentDataModel> GetMonumentAsync(Guid monumentId);

        Task<MonumentImagesDataModel> GetMonumentImagesAsync(Guid monumentId);

        Task CreateMonumentAsync(MonumentDataModel monument);

        Task UpdateMonumentAsync(MonumentDataModel monument);

        Task UpdateMonumentImagesAsync(MonumentImagesDataModel monumentImages);

        Task DeleteMonument(Guid monumentId);

        Task UpdateRouteMonumentsAsync(RouteMonumentsDataModel routeMonuments);

        Task<RouteMonumentsDataModel> GetRouteMonumentsAsync(Guid routeId);
    }
}
