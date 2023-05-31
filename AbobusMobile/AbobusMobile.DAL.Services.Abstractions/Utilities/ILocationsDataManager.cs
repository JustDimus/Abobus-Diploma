using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Abstractions.Utilities
{
    public interface ILocationsDataManager
    {
        Task<bool> CheckLocationAvailabilityAsync(Guid cityId);
        Task<LocationDataModel> GetLocationAsync(Guid cityId);

        Task<List<LocationDataModel>> FindLocations(string cityNamePattern);

        Task DeleteLocation(Guid cityId);

        Task CreateLocation(LocationDataModel locationDataModel);
    }
}
