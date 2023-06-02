using AbobusMobile.BLL.Services.Abstractions.Utitlities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Abstractions.Utilities
{
    public interface ILocationService
    {
        Task<LocationServiceModel> GetCurrentLocationAsync();

        Task<LocationServiceModel> GetLocationAsync(Guid locationId);

        Task<List<LocationServiceModel>> GetLocationsAsync(string cityNamePattern);

        Task<LocationCoordinatesServiceModel> GetCurrentLocationCoordinatesAsync();
    }
}
