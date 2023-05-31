using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Abstractions.Monuments
{
    public interface IMonumentsService
    {
        Task<MonumentServiceModel> GetMonumentAsync(Guid monumentId);

        Task<List<MonumentServiceModel>> GetMonumentsByRouteIdAsync(Guid routeId);

        Task<List<MonumentServiceModel>> GetMonumentsByCityIdAsync(Guid cityId);

        Task<List<MonumentImageServiceModel>> GetMonumentImagesAsync(Guid monuments);
    }
}
