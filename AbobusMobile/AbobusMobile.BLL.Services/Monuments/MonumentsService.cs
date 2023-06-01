using AbobusCore.Models.Monuments;
using AbobusMobile.BLL.Services.Abstractions.Monuments;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.Communication.Requests.Monuments;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.DAL.Services.Abstractions.Monuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Monuments
{
    public class MonumentsService : IMonumentsService
    {
        private readonly IRequestFactory _requestFactory;
        private readonly IMonumentsDataManager _monumentsManager;
        private readonly IResourcesService _resourcesService;

        private GetMonumentImagesRequest monumentImagesRequest;
        private GetMonumentRequest monumentRequest;
        private GetMonumentsByRouteId monumentsByRouteId;

        public MonumentsService(
            IRequestFactory requestFactory,
            IMonumentsDataManager monumentsDataManager,
            IResourcesService resourcesService)
        {
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
            _monumentsManager = monumentsDataManager ?? throw new ArgumentNullException(nameof(monumentsDataManager));
            _resourcesService = resourcesService ?? throw new ArgumentNullException(nameof(resourcesService));
        }

        private GetMonumentImagesRequest MonumentImagesRequest
            => monumentImagesRequest ?? (monumentImagesRequest = _requestFactory.CreateRequest<GetMonumentImagesRequest>());
        private GetMonumentRequest MonumentRequest
            => monumentRequest ?? (monumentRequest = _requestFactory.CreateRequest<GetMonumentRequest>());
        private GetMonumentsByRouteId MonumentsByRouteId
            => monumentsByRouteId ?? (monumentsByRouteId = _requestFactory.CreateRequest<GetMonumentsByRouteId>());

        public async Task<MonumentServiceModel> GetMonumentAsync(Guid monumentId)
        {
            var monumentDownloaded = await _monumentsManager.CheckMonumentAvailabilityAsync(monumentId);

            MonumentServiceModel result = null;

            var monumentDataModel = await _monumentsManager.GetMonumentAsync(monumentId);

            result = GetMonumentServiceModel(monumentDataModel);

            MonumentRequest.Initialize(monumentId);

            var monumentResponse = await MonumentRequest.SendRequestAsync();

            if (monumentResponse.Succeeded)
            {
                var newMonumentModel = monumentResponse.As<MonumentModel>();

                MonumentImagesRequest.Initialize(monumentId);

                var imagesResponse = await MonumentImagesRequest.SendRequestAsync();

                if (imagesResponse.Succeeded)
                {
                    var monumentImages = imagesResponse.As<List<MonumentImageModel>>();

                    foreach (var image in monumentImages)
                    {
                        await _resourcesService.DownloadResourceIfNeededAsync(image.ImageId);
                    }

                    await _monumentsManager.UpdateMonumentImagesAsync(GetMonumentImageDataModel(monumentId, monumentImages));
                }

                if (monumentDownloaded)
                {
                    await _monumentsManager.UpdateMonumentAsync(GetMonumentDataModel(newMonumentModel));
                }
                else
                {
                    await _monumentsManager.CreateMonumentAsync(GetMonumentDataModel(newMonumentModel));
                }

                result = GetMonumentServiceModel(newMonumentModel);
            }

            return result;
        }

        public async Task<List<MonumentImageServiceModel>> GetMonumentImagesAsync(Guid monumentId)
        {
            var monumentImages = await _monumentsManager.GetMonumentImagesAsync(monumentId);

            var result = new List<MonumentImageServiceModel>();

            if (result != null)
            {
                foreach (var monumentImage in monumentImages.MonumentImagesId)
                {
                    result.Add(new MonumentImageServiceModel()
                    {
                        MonumentId = monumentId,
                        ImageId = monumentImage,
                    });
                }
            }

            return result;
        }

        public Task<List<MonumentServiceModel>> GetMonumentsByCityIdAsync(Guid cityId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MonumentServiceModel>> GetMonumentsByRouteIdAsync(Guid routeId)
        {
            var routeMonuments = await _monumentsManager.GetRouteMonumentsAsync(routeId);

            List<Guid> requestedMonuments = routeMonuments.MonumentsId;

            MonumentsByRouteId.Initialize(routeId);

            var monumentsResponse = await MonumentsByRouteId.SendRequestAsync();

            if (monumentsResponse.Succeeded)
            {
                var newMonumentsId = monumentsResponse.As<List<MonumentRouteModel>>();

                await _monumentsManager.UpdateRouteMonumentsAsync(GetRouteMonumentsDataModel(routeId, newMonumentsId));

                requestedMonuments.Clear();
                requestedMonuments.AddRange(newMonumentsId.Select(i => i.MonumentId));
            }

            var result = new List<MonumentServiceModel>();

            foreach (var monumentId in requestedMonuments)
            {
                result.Add(await GetMonumentAsync(monumentId));
            }

            return result;
        }

        private RouteMonumentsDataModel GetRouteMonumentsDataModel(Guid routeId, List<MonumentRouteModel> monuments)
            => new RouteMonumentsDataModel()
            {
                RouteId = routeId,
                MonumentsId = monuments.Select(i => i.MonumentId).ToList(),
            };

        private MonumentDataModel GetMonumentDataModel(MonumentModel monument)
            => new MonumentDataModel()
            {
                Id = monument.Id,
                Description = monument.Description,
                MonumentTitleImageId = monument.MonumentTitleImageId,
                Name = monument.Name,
                CityId = monument.CityId,
            };

        private MonumentServiceModel GetMonumentServiceModel(MonumentModel monument)
            => new MonumentServiceModel()
            {
                Id = monument.Id,
                Description = monument.Description,
                MonumentTitleImageId = monument.MonumentTitleImageId,
                Name = monument.Name,
                CityId = monument.CityId,
            };

        private MonumentServiceModel GetMonumentServiceModel(MonumentDataModel monument)
            => new MonumentServiceModel()
            {
                Id = monument.Id,
                Description = monument.Description,
                MonumentTitleImageId = monument.Id,
                Name = monument.Name,
                CityId = monument.CityId,
            };

        private MonumentImagesDataModel GetMonumentImageDataModel(Guid monumentId, List<MonumentImageModel> monumentImages)
            => new MonumentImagesDataModel()
            {
                MonumentId = monumentId,
                MonumentImagesId = monumentImages.Select(i => i.ImageId).ToList(),
            };
    }
}
