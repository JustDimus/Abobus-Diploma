using AbobusMobile.DAL.Services.Abstractions.Utilities;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Utilities
{
    public class LocationsDataManager : ILocationsDataManager
    {
        private IRepository<LocationModel> _locations;

        public LocationsDataManager(
            IRepository<LocationModel> locations)
        {
            _locations = locations ?? throw new ArgumentNullException(nameof(locations));
        }

        public async Task<bool> CheckLocationAvailabilityAsync(Guid cityId)
        {
            return await _locations.AnyAsync(i => i.CityId == cityId);
        }

        public async Task CreateLocation(LocationDataModel locationDataModel)
        {
            var existingLocation = await _locations.FirstOrDefaultAsync(i => i.CityId == locationDataModel.CityId);

            if (existingLocation != null)
            {
                existingLocation.CityName = locationDataModel.CityName;

                await _locations.UpdateAsync(existingLocation);
            }
            else
            {
                var newLocation = ToDbModel(locationDataModel);

                await _locations.InsertAsync(newLocation);
            }
        }

        public async Task DeleteLocation(Guid cityId)
        {
            var location = await _locations.FirstOrDefaultAsync(i => i.CityId == cityId);

            if (location != null)
            {
                await _locations.DeleteAsync(location);
            }
        }

        public async Task<List<LocationDataModel>> FindLocations(string cityNamePattern)
        {
            var locations = await _locations.SelectAsync(i => i.CityName.Contains(cityNamePattern));

            return locations.Select(ToDataModel).ToList();
        }

        public async Task<LocationDataModel> GetLocationAsync(Guid cityId)
        {
            var location = await _locations.FirstOrDefaultAsync(i => i.CityId == cityId);

            if (location != null)
            {
                return ToDataModel(location);
            }

            return null;
        }

        private LocationDataModel ToDataModel(LocationModel model)
            => new LocationDataModel()
            {
                CityId = model.CityId,
                CityName = model.CityName
            };

        private LocationModel ToDbModel(LocationDataModel model)
            => new LocationModel()
            {
                CityId = model.CityId,
                CityName = model.CityName
            };
    }
}
