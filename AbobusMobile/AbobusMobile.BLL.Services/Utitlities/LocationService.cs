using AbobusCore.Models.Locations;
using AbobusMobile.BLL.Services.Abstractions.Utilities;
using AbobusMobile.Communication.Requests.Locations;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.DAL.Services.Abstractions.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AbobusMobile.BLL.Services.Utitlities
{
    public class LocationService : ILocationService
    {
        private IRequestFactory _requestFactory;
        private ILocationsDataManager _locationsManager;

        private GetLocationByCoordinatesRequest locationByCoordinatesRequest = null;
        private GetLocationByIdRequest locationByIdRequest = null;
        private GetLocationsRequest getLocationsRequest = null;

        public LocationService(
            IRequestFactory requestFactory,
            ILocationsDataManager locationsManager)
        {
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
            _locationsManager = locationsManager ?? throw new ArgumentNullException(nameof(locationsManager));
        }

        private GetLocationByIdRequest LocationByIdRequest
            => locationByIdRequest ?? (locationByIdRequest = _requestFactory.CreateRequest<GetLocationByIdRequest>());

        private GetLocationByCoordinatesRequest LocationByCoordinatesRequest
            => locationByCoordinatesRequest ?? (locationByCoordinatesRequest = _requestFactory.CreateRequest<GetLocationByCoordinatesRequest>());

        private GetLocationsRequest GetLocationsRequest
            => getLocationsRequest ?? (getLocationsRequest = _requestFactory.CreateRequest<GetLocationsRequest>());

        public async Task<LocationServiceModel> GetCurrentLocationAsync()
        {
            var deviceLocation = await GetLocationAsync();

            LocationByCoordinatesRequest.Initialize(deviceLocation.Longitude, deviceLocation.Latitude);

            var locationResponse = await LocationByCoordinatesRequest.SendRequestAsync();

            LocationServiceModel result = null;

            if (locationResponse.Succeeded)
            {
                var locationData = locationResponse.As<LocationDetailsModel>();

                if (locationData.LocationFound)
                {
                    await SaveLocationToCache(locationData);
                }

                result = GetLocationServiceModel(locationData);
            }

            return result;
        }

        public async Task<List<LocationServiceModel>> GetLocationsAsync(string cityNamePattern)
        {
            GetLocationsRequest.Initialize(cityNamePattern);

            var locationsResponse = await GetLocationsRequest.SendRequestAsync();

            var result = new List<LocationServiceModel>();

            var cachedLocations = await _locationsManager.FindLocations(cityNamePattern);

            result.AddRange(cachedLocations.Select(GetLocationServiceModel));

            if (locationsResponse.Succeeded)
            {
                var locations = locationsResponse.As<List<LocationDetailsModel>>();

                foreach (var location in locations)
                {
                    await SaveLocationToCache(location);

                    if (!result.Any(i => i.CityId == location.CityId))
                    {
                        result.Add(GetLocationServiceModel(location));
                    }
                }
            }

            return result;
        }

        public async Task<LocationServiceModel> GetLocationAsync(Guid locationId)
        {
            LocationByIdRequest.Initialize(locationId);

            var locationResponse = await LocationByIdRequest.SendRequestAsync();

            LocationServiceModel result = null;

            if (locationResponse.Succeeded)
            {
                var locationData = locationResponse.As<LocationDetailsModel>();

                if (locationData.LocationFound)
                {
                    await SaveLocationToCache(locationData);
                }

                result = GetLocationServiceModel(locationData);
            }

            return result;
        }

        private async Task SaveLocationToCache(LocationDetailsModel locationData)
        {
            var locationExist = await _locationsManager.CheckLocationAvailabilityAsync(locationData.CityId.Value);

            if (!locationExist)
            {
                await _locationsManager.CreateLocation(new LocationDataModel()
                {
                    CityId = locationData.CityId.Value,
                    CityName = locationData.CityName,
                });
            }
        }

        private LocationServiceModel GetLocationServiceModel(LocationDataModel location)
            => new LocationServiceModel()
            {
                CityId = location.CityId,
                CityName = location.CityName,
                LocationFound = true
            };

        private LocationServiceModel GetLocationServiceModel(LocationDetailsModel location)
            => new LocationServiceModel()
            {
                CityId = location.CityId.GetValueOrDefault(),
                CityName = location.CityName,
                LocationFound = location.LocationFound
            };

        private Task<Location> GetLocationAsync()
        {
            try
            {
                var deviceLocation = Geolocation.GetLastKnownLocationAsync();

                if (deviceLocation == null)
                {
                    using (CancellationTokenSource source = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                    {
                        deviceLocation = Geolocation.GetLocationAsync(new GeolocationRequest()
                        {
                            DesiredAccuracy = GeolocationAccuracy.Medium,
                            Timeout = new TimeSpan(0, 0, 30)
                        }, source.Token);
                    }
                }

                return deviceLocation;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
