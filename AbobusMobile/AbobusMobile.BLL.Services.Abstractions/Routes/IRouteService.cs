using AbobusMobile.BLL.Services.Abstractions.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Abstractions.Routes
{
    public interface IRouteService
    {
        Task<List<RouteDetailsServiceModel>> GetRoutesDetailsByCityId(Guid cityId);

        Task<ResourceServiceStatus> GetRouteStatusAsync(Guid routeId);

        Task<ResourceServiceStatus> DownloadRouteAsync(Guid routeId);

        Task<ResourceServiceStatus> DeleteRouteAsync(Guid routeId);

        Task<RouteDetailsServiceModel> GetRouteDetailsAsync(Guid routeId);

        Task<Stream> GetRouteImageAsync(Guid routeId);

        Task<Stream> GetRouteResourceAsync(Guid routeId);
    }
}
