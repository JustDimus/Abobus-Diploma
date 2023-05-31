using AbobusMobile.DAL.Services.Abstractions.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Utilities
{
    public class LocationsDataManager : ILocationsDataManager
    {
        public Task<bool> CheckLocationAvailabilityAsync(Guid cityId)
        {
            throw new NotImplementedException();
        }

        public Task CreateLocation(LocationDataModel locationDataModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLocation(Guid cityId)
        {
            throw new NotImplementedException();
        }

        public Task<List<LocationDataModel>> FindLocations(string cityNamePattern)
        {
            throw new NotImplementedException();
        }

        public Task<LocationDataModel> GetLocationAsync(Guid cityId)
        {
            throw new NotImplementedException();
        }
    }
}
