using AbobusCore.Models.Routes;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.BLL.Services.Abstractions.Routes;
using AbobusMobile.BLL.Services.Abstractions.Utilities;
using AbobusMobile.Communication.Requests.Resources;
using AbobusMobile.Communication.Requests.Routes;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Abstractions.Extensions;
using AbobusMobile.DAL.Services.Abstractions.Routes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Routes
{
    public class RouteService : IRouteService
    {
        private readonly IRequestFactory _requestFactory;
        private readonly IRoutesDataManager _routesManager;
        private readonly IResourcesService _resourceService;
        private readonly ILocationService _locationService;

        private GetRouteDetailsRequest detailsRequest;
        private GetRoutesDetailsRequest routesRequest;

        public RouteService(
            IRoutesDataManager routesManager,
            IRequestFactory requestFactory,
            IResourcesService resourcesService,
            ILocationService locationsService)
        {
            _routesManager = routesManager ?? throw new ArgumentNullException(nameof(routesManager));
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
            _resourceService = resourcesService ?? throw new ArgumentNullException(nameof(resourcesService));
            _locationService = locationsService ?? throw new ArgumentNullException(nameof(locationsService));
        }

        public GetRouteDetailsRequest DetailsRequest
            => detailsRequest ?? (detailsRequest = _requestFactory.CreateRequest<GetRouteDetailsRequest>());

        public GetRoutesDetailsRequest RoutesRequest
            => routesRequest ?? (routesRequest = _requestFactory.CreateRequest<GetRoutesDetailsRequest>());

        public async Task<ResourceServiceStatus> DeleteRouteAsync(Guid routeId)
        {
            var routeExist = await _routesManager.CheckRouteAvailabilityAsync(routeId);

            if (routeExist)
            {
                var routeDetails = await _routesManager.GetAsync(routeId);

                await _resourceService.DeleteResourceAsync(routeDetails.RouteResourceId);
                await _resourceService.DeleteResourceAsync(routeDetails.RouteImageId);

                await _routesManager.DeleteAsync(routeDetails.Id);
            }

            return ResourceServiceStatus.Deleted;
        }

        public async Task<ResourceServiceStatus> GetRouteStatusAsync(Guid routeId)
        {
            var routeDetailsDownloaded = await _routesManager.CheckRouteAvailabilityAsync(routeId);

            if (routeDetailsDownloaded)
            {
                var routeDetails = await _routesManager.GetAsync(routeId);

                var routeResourceStatus = await _resourceService.GetResourceStatusAsync(routeDetails.RouteResourceId);
                var routeImageStatus = await _resourceService.GetResourceStatusAsync(routeDetails.RouteImageId);

                if (routeResourceStatus == ResourceServiceStatus.Downloaded
                    && routeImageStatus == ResourceServiceStatus.Downloaded)
                {
                    return ResourceServiceStatus.Downloaded;
                }
            }

            DetailsRequest.Initialize(routeId);

            var detailsResponse = await DetailsRequest.SendRequestAsync();

            if (detailsResponse.NotFound())
            {
                return ResourceServiceStatus.NotFound;
            }

            if (detailsResponse.Succeeded)
            {
                return ResourceServiceStatus.Available;
            }


            return ResourceServiceStatus.Unknown;
        }

        public async Task<ResourceServiceStatus> DownloadRouteAsync(Guid routeId)
        {
            DetailsRequest.Initialize(routeId);

            var detailsResponse = await DetailsRequest.SendRequestAsync();

            if (detailsResponse.Succeeded)
            {
                var routeDetails = detailsResponse.As<RouteDetailsModel>();

                var routeLocationData = await _locationService.GetLocationAsync(routeDetails.CityId);

                var resourceDownloadStatus = await _resourceService.DownloadResourceIfNeededAsync(routeDetails.RouteResourceId);

                var imageDownloadStatus = await _resourceService.DownloadResourceIfNeededAsync(routeDetails.RouteResourceId);

                if (resourceDownloadStatus == ResourceServiceStatus.Downloaded
                    && imageDownloadStatus == ResourceServiceStatus.Downloaded
                    && routeLocationData.LocationFound)
                {
                    await _routesManager.UpdateAsync(GetRouteDataModel(routeDetails));

                    return ResourceServiceStatus.Downloaded;
                }
                else
                {
                    await _resourceService.DeleteResourceAsync(routeDetails.RouteResourceId);
                    await _resourceService.DeleteResourceAsync(routeDetails.RouteImageId);
                }
            }

            return ResourceServiceStatus.DownloadFailed;
        }

        public async Task<List<RouteDetailsServiceModel>> GetRoutesDetailsByCityId(Guid cityId)
        {
            var result = new List<RouteDetailsServiceModel>();

            var downloadedRoutes = await LoadDownloadedRoutes(cityId);
            result.AddRange(downloadedRoutes);

            var availableResource = await LoadAvailableRoutes(cityId);
            result.AddRange(availableResource);

            return result;
        }

        public async Task<RouteDetailsServiceModel> GetRouteDetailsAsync(Guid routeId)
        {
            RouteDetailsServiceModel result = null;

            var downloadedRouteExist = await _routesManager.CheckRouteAvailabilityAsync(routeId);

            if (downloadedRouteExist)
            {
                var downloadedRoute = await _routesManager.GetAsync(routeId);

                result = GetRouteServiceModel(downloadedRoute);
            }

            DetailsRequest.Initialize(routeId);

            var detailsResponse = await DetailsRequest.SendRequestAsync();

            if (detailsResponse.Succeeded)
            {
                var routeDetails = detailsResponse.As<RouteDetailsModel>();

                await _locationService.GetLocationAsync(routeDetails.CityId);

                ResourceServiceStatus updateStatus = ResourceServiceStatus.Unknown;

                updateStatus = await _resourceService.DownloadResourceIfNeededAsync(routeDetails.RouteImageId);

                if (updateStatus != ResourceServiceStatus.Downloaded)
                {
                    throw new InvalidOperationException("Image can not be downloaded");
                }

                if (downloadedRouteExist
                    && routeDetails.RouteResourceId != result.RouteResourceId)
                {
                    updateStatus = await _resourceService.DownloadResourceIfNeededAsync(routeDetails.RouteResourceId);

                    if (updateStatus != ResourceServiceStatus.Downloaded)
                    {
                        throw new InvalidOperationException("Resource can not be downloaded");
                    }
                }

                if (downloadedRouteExist)
                {
                    await _routesManager.UpdateAsync(GetRouteDataModel(routeDetails));
                }

                result = GetRouteServiceModel(routeDetails);
                result.Downloaded = downloadedRouteExist;
            }

            return result;
        }

        public async Task<Stream> GetRouteImageAsync(Guid routeId)
        {
            var routeDetails = await GetRouteDetailsAsync(routeId);

            if (!routeDetails.Downloaded)
            {
                var routeImageStatus = await _resourceService.DownloadResourceIfNeededAsync(routeDetails.RouteImageId);

                if (routeImageStatus != ResourceServiceStatus.Downloaded)
                {
                    throw new InvalidOperationException("Could not load route image");
                }
            }

            var result = await _resourceService.GetResourceAsync(routeDetails.RouteImageId);

            return result.Resource;
        }

        public async Task<Stream> GetRouteResourceAsync(Guid routeId)
        {
            var routeDetails = await GetRouteDetailsAsync(routeId);

            if (!routeDetails.Downloaded)
            {
                var routeResourceStatus = await _resourceService.DownloadResourceIfNeededAsync(routeDetails.RouteResourceId);

                if (routeResourceStatus != ResourceServiceStatus.Downloaded)
                {
                    throw new InvalidOperationException("Could not load route resource");
                }
            }

            var result = await _resourceService.GetResourceAsync(routeDetails.RouteResourceId);

            return result.Resource;
        }

        private async Task<IEnumerable<RouteDetailsServiceModel>> LoadDownloadedRoutes(Guid cityId)
        {
            var downloadedRoutes = await _routesManager.GetAllAsync(cityId);

            return downloadedRoutes.Select(GetRouteServiceModel);
        }

        private async Task<IEnumerable<RouteDetailsServiceModel>> LoadAvailableRoutes(Guid cityId)
        {
            RoutesRequest.Initialize(cityId);

            var routesResponse = await RoutesRequest.SendRequestAsync();

            var result = new List<RouteDetailsServiceModel>();

            if (routesResponse.Succeeded)
            {
                var routesDetails = routesResponse.As<List<RouteDetailsModel>>();

                foreach (var route in routesDetails)
                {
                    var routeImageStatus = await _resourceService.DownloadResourceIfNeededAsync(route.RouteImageId);

                    if (routeImageStatus == ResourceServiceStatus.Downloaded)
                    {
                        result.Add(GetRouteServiceModel(route));
                    }
                }
            }

            return result;
        }

        private RouteDataModel GetRouteDataModel(RouteDetailsModel detailsModel)
            => new RouteDataModel()
            {
                Id = detailsModel.Id,
                CreatorId = detailsModel.CreatorId,
                Distance = detailsModel.Distance,
                RouteImageId = detailsModel.RouteImageId,
                DistanceUnit = detailsModel.DistanceUnit,
                Name = detailsModel.Name,
                RouteResourceId = detailsModel.RouteResourceId,
                CityId = detailsModel.CityId,
            };

        private RouteDetailsServiceModel GetRouteServiceModel(RouteDetailsModel detailsModel)
            => new RouteDetailsServiceModel()
            {
                Id = detailsModel.Id,
                CreatorId = detailsModel.CreatorId,
                Distance = detailsModel.Distance,
                DistanceUnit = detailsModel.DistanceUnit,
                Downloaded = false,
                Name = detailsModel.Name,
                RouteImageId = detailsModel.RouteImageId,
                RouteResourceId = detailsModel.RouteResourceId,
                CityId = detailsModel.CityId,
            };

        private RouteDetailsServiceModel GetRouteServiceModel(RouteDataModel dataModel)
            => new RouteDetailsServiceModel()
            {
                Id = dataModel.Id,
                CreatorId = dataModel.CreatorId,
                Distance = dataModel.Distance,
                DistanceUnit = dataModel.DistanceUnit,
                Downloaded = true,
                Name = dataModel.Name,
                RouteImageId = dataModel.RouteImageId,
                RouteResourceId = dataModel.RouteResourceId,
                CityId = dataModel.CityId,
            };
    }
}
