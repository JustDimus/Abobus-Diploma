using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Abstractions.Utilities
{
    public interface ILocationService
    {
        Task<LocationServiceModel> GetCurrentLocationAsync();

        Task<List<LocationServiceModel>> GetLocations(string cityNamePattern);

    }
}
