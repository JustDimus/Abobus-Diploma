using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.DAL.Services.Abstractions.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Resources
{
    public class ResourcesService : IResourcesService
    {
        private IResourcesDataManager _resourcesManager;
        private IRequestFactory _requestFactory;

        public ResourcesService(
            IRequestFactory requestFactory,
            IResourcesDataManager resourcesManager)
        {
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
            _resourcesManager = resourcesManager ?? throw new ArgumentNullException(nameof(resourcesManager));
        }

        public Task<ResourceServiceStatus> DeleteResourceAsync(Guid resourceId)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceServiceStatus> DownloadResourceAsync(Guid resourceId)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceServiceModel> GetResourceAsync(Guid resourceId)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceServiceStatus> GetResourceStatusAsync(Guid resourceId)
        {
            throw new NotImplementedException();
        }
    }
}
