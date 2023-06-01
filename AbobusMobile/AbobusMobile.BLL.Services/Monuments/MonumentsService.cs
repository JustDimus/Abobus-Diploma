using AbobusMobile.BLL.Services.Abstractions.Monuments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Monuments
{
    public class MonumentsService : IMonumentsService
    {
        public Task<MonumentServiceModel> GetMonumentAsync(Guid monumentId)
        {
            throw new NotImplementedException();
        }

        public Task<List<MonumentImageServiceModel>> GetMonumentImagesAsync(Guid monuments)
        {
            throw new NotImplementedException();
        }

        public Task<List<MonumentServiceModel>> GetMonumentsByCityIdAsync(Guid cityId)
        {
            throw new NotImplementedException();
        }

        public Task<List<MonumentServiceModel>> GetMonumentsByRouteIdAsync(Guid routeId)
        {
            throw new NotImplementedException();
        }
    }
}
