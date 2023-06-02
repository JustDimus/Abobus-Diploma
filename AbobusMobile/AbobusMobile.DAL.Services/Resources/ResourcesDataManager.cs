using AbobusMobile.DAL.Services.Abstractions.Resources;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Resources
{
    public class ResourcesDataManager : IResourcesDataManager
    {
        private IRepository<ResourceModel> _resources;
        private ResourcesDirectoryManager _resourcesManager;

        public ResourcesDataManager(
            ResourcesDirectoryManager resourcesManager,
            IRepository<ResourceModel> resources)
        {
            _resourcesManager = resourcesManager ?? throw new ArgumentNullException(nameof(resourcesManager));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public async Task<bool> CheckAvailability(Guid resourceId)
        {
            var resource = await GetResourceModel(resourceId);

            if (resource != null)
            {
                return await _resourcesManager.CheckResourceFileAvailabilityAsync(resource.Path);
            }

            return false;
        }

        public async Task CreateAsync(CreateResourceDataModel resource)
        {
            var resourceIdIsBusy = await _resources.AnyAsync(i => i.GlobalId == resource.GlobalId);

            if (resourceIdIsBusy)
            {
                await DeleteAsync(resource.GlobalId);
            }

            var newResource = new ResourceModel()
            {
                GlobalId = resource.GlobalId,
                Name = resource.Name,
                Path = resource.Name
            };

            var resourceNameIsBusy = await _resources.AnyAsync(i => i.Path == newResource.Path);

            resourceNameIsBusy = resourceNameIsBusy
                || await _resourcesManager.CheckResourceFileAvailabilityAsync(newResource.Path);

            if (resourceNameIsBusy)
            {
                newResource.Path = string.Concat(Guid.NewGuid(), newResource.Path);
            }

            await _resourcesManager.WriteResourceFileAsync(newResource.Path, resource.SourceStream);

            await _resources.InsertAsync(newResource);
        }

        public async Task DeleteAsync(Guid resourceId)
        {
            var resourceExist = await _resources.AnyAsync(i => i.GlobalId == resourceId);

            if (resourceExist)
            {
                var resource = await GetResourceModel(resourceId);

                var resourceFileExist = await _resourcesManager.CheckResourceFileAvailabilityAsync(resource.Path);

                if (resourceFileExist)
                {
                    await _resourcesManager.DeleteResourceFileAsync(resource.Path);
                }

                await _resources.DeleteAsync(resource);
            }
        }

        public async Task<ResourceDataModel> GetAsync(Guid resourceId)
        {
            var resource = await GetResourceModel(resourceId);

            ResourceDataModel result = null;

            if (resource != null)
            {
                result = new ResourceDataModel()
                {
                    GlobalId = resource.GlobalId,
                    Name = resource.Name,
                };
            }

            return result;
        }

        public async Task<MemoryStream> LoadAsync(Guid resourceId)
        {
            var resourceExist = await CheckAvailability(resourceId);

            MemoryStream result = null;

            if (resourceExist)
            {
                var resource = await GetResourceModel(resourceId);

                var resourceFileExist = await _resourcesManager.CheckResourceFileAvailabilityAsync(resource.Path);

                if (resourceFileExist)
                {
                    result = await _resourcesManager.LoadResourceFileAsync(resource.Path);
                }
            }

            return result;
        }

        private async Task<ResourceModel> GetResourceModel(Guid resourceId)
            => await _resources.FirstOrDefaultAsync(i => i.GlobalId == resourceId);
    }
}
