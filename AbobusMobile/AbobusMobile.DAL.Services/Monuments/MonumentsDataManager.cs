using AbobusMobile.DAL.Services.Abstractions.Monuments;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.Abstractions;
using Nancy.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Monuments
{
    public class MonumentsDataManager : IMonumentsDataManager
    {
        private readonly IRepository<MonumentModel> _monuments;
        private readonly IRepository<MonumentImageModel> _monumentImages;
        private readonly IRepository<RouteMonumentModel> _routeMonuments;

        public MonumentsDataManager(
            IRepository<MonumentModel> monumentsRepository,
            IRepository<MonumentImageModel> monumentImagesRepository,
            IRepository<RouteMonumentModel> routeMonumentsRepository)
        {
            _monuments = monumentsRepository ?? throw new ArgumentNullException(nameof(monumentsRepository));
            _monumentImages = monumentImagesRepository ?? throw new ArgumentNullException(nameof(monumentImagesRepository));
            _routeMonuments = routeMonumentsRepository ?? throw new ArgumentNullException(nameof(routeMonumentsRepository));
        }

        public async Task<bool> CheckMonumentAvailabilityAsync(Guid monumentId)
        {
            return await _monuments.AnyAsync(i => i.MonumentId == monumentId);
        }

        public async Task CreateMonumentAsync(MonumentDataModel monument)
        {
            var monumentEntity = new MonumentModel()
            {
                Description = monument.Description,
                MonumentId = monument.Id,
                MonumentTitleImageId = monument.MonumentTitleImageId,
                Name = monument.Name,
                CityId = monument.CityId,
            };

            await _monuments.InsertAsync(monumentEntity);
        }

        public async Task DeleteMonument(Guid monumentId)
        {
            var monumentEntity = await _monuments.FirstOrDefaultAsync(i => i.MonumentId == monumentId);

            if (monumentEntity != null)
            {
                await _monuments.DeleteAsync(monumentEntity);
            }
        }

        public async Task<MonumentDataModel> GetMonumentAsync(Guid monumentId)
        {
            var monumentEntity = await _monuments.FirstOrDefaultAsync(i => i.MonumentId == monumentId);

            if (monumentEntity != null)
            {
                return new MonumentDataModel()
                {
                    Description = monumentEntity.Description,
                    Id = monumentEntity.MonumentId,
                    MonumentTitleImageId = monumentEntity.MonumentTitleImageId,
                    Name = monumentEntity.Name,
                    CityId = monumentEntity.CityId,
                };
            }

            return null;
        }

        public async Task<MonumentImagesDataModel> GetMonumentImagesAsync(Guid monumentId)
        {
            var result = new MonumentImagesDataModel()
            {
                MonumentId = monumentId,
                MonumentImagesId = new List<Guid>()
            };

            var monumentImages = await _monumentImages.SelectAsync(i => i.MonumentId == monumentId);

            foreach (var monumentImage in monumentImages)
            {
                result.MonumentImagesId.Add(monumentImage.ImageId);
            }

            return result;
        }

        public async Task<RouteMonumentsDataModel> GetRouteMonumentsAsync(Guid routeId)
        {
            var result = new RouteMonumentsDataModel()
            {
                RouteId = routeId,
                MonumentsId = new List<Guid>()
            };

            var routeMonuments = await _routeMonuments.SelectAsync(i => i.RouteId == routeId);

            foreach (var routeMonument in routeMonuments)
            {
                result.MonumentsId.Add(routeMonument.MonumentId);
            }

            return result;
        }

        public async Task UpdateMonumentAsync(MonumentDataModel monument)
        {
            var monumentExist = await _monuments.AnyAsync(i => i.MonumentId == monument.Id);

            if (monumentExist)
            {
                await DeleteMonument(monument.Id);
            }

            await CreateMonumentAsync(monument);
        }

        public async Task UpdateMonumentImagesAsync(MonumentImagesDataModel monumentImages)
        {
            var existingMonumentImages = await _monumentImages.SelectAsync(i => i.MonumentId == monumentImages.MonumentId);

            foreach (var newMonumentImageId in monumentImages.MonumentImagesId)
            {
                if (!existingMonumentImages.Any(i
                    => i.MonumentId == monumentImages.MonumentId
                    && i.ImageId == newMonumentImageId))
                {
                    await _monumentImages.InsertAsync(new MonumentImageModel()
                    {
                        ImageId = newMonumentImageId,
                        MonumentId = monumentImages.MonumentId,
                    });
                }
            }

            foreach (var image in existingMonumentImages.Where(i
                => !monumentImages.MonumentImagesId.Contains(i.ImageId)))
            {
                await _monumentImages.DeleteAsync(image);
            }
        }

        public async Task UpdateRouteMonumentsAsync(RouteMonumentsDataModel routeMonuments)
        {
            var existingRouteMonuments = await _routeMonuments.SelectAsync(i => i.RouteId == routeMonuments.RouteId);

            foreach (var newRouteMonumentId in routeMonuments.MonumentsId)
            {
                if (!existingRouteMonuments.Any(i
                    => i.MonumentId == newRouteMonumentId
                    && i.RouteId == routeMonuments.RouteId))
                {
                    await _routeMonuments.InsertAsync(new RouteMonumentModel()
                    {
                        RouteId = routeMonuments.RouteId,
                        MonumentId = newRouteMonumentId,
                    });
                }
            }

            foreach (var monument in existingRouteMonuments.Where(i
                => !routeMonuments.MonumentsId.Contains(i.MonumentId)))
            {
                await _routeMonuments.DeleteAsync(monument);
            }
        }
    }
}
