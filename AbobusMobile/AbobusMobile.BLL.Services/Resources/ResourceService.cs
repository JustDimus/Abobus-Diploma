using AbobusCore.Models.Resources;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.Communication.Requests.Resources;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Abstractions.Extensions;
using AbobusMobile.DAL.Services.Abstractions.Resources;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Resources
{
    public class ResourcesService : IResourcesService
    {
        private IResourcesDataManager _resourcesManager;
        private IRequestFactory _requestFactory;

        private GetResourceDetailsRequest resourceDetailsRequest;
        private DownloadResourceRequest downloadResourceRequest;

        public ResourcesService(
            IRequestFactory requestFactory,
            IResourcesDataManager resourcesManager)
        {
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
            _resourcesManager = resourcesManager ?? throw new ArgumentNullException(nameof(resourcesManager));
        }

        private GetResourceDetailsRequest ResourceDetailsRequest
            => resourceDetailsRequest ?? (resourceDetailsRequest = _requestFactory.CreateRequest<GetResourceDetailsRequest>());

        private DownloadResourceRequest DownloadResourceRequest
            => downloadResourceRequest ?? (downloadResourceRequest = _requestFactory.CreateRequest<DownloadResourceRequest>());

        public async Task<ResourceServiceStatus> DeleteResourceAsync(Guid resourceId)
        {
            await _resourcesManager.DeleteAsync(resourceId);

            return ResourceServiceStatus.Deleted;
        }

        public async Task<ResourceServiceStatus> DownloadResourceAsync(Guid resourceId)
        {
            ResourceDetailsRequest.Initialize(resourceId);

            var resourceResponse = await ResourceDetailsRequest.SendRequestAsync();

            if (resourceResponse.Succeeded)
            {
                var resourceDetails = resourceResponse.As<ResourcesDetailsModel>();

                DownloadResourceRequest.Initialize(resourceId);

                var downloadResponse = await DownloadResourceRequest.SendRequestAsync();

                if (downloadResponse.Succeeded)
                {
                    await _resourcesManager.CreateAsync(new CreateResourceDataModel()
                    {
                        GlobalId = resourceId,
                        Name = resourceDetails.Name,
                        SourceStream = downloadResponse.AsStream()
                    });

                    return ResourceServiceStatus.Downloaded;
                }
            }

            return ResourceServiceStatus.DownloadFailed;
        }

        public async Task<ResourceServiceStatus> DownloadResourceIfNeededAsync(Guid resourceId)
        {
            var resourceDownloadStatus = await GetResourceStatusAsync(resourceId);

            if (resourceDownloadStatus != ResourceServiceStatus.Downloaded)
            {
                resourceDownloadStatus = await DownloadResourceAsync(resourceId);
            }

            return resourceDownloadStatus;
        }

        public async Task<ResourceServiceModel> GetResourceAsync(Guid resourceId)
        {
            var resourceStatus = await DownloadResourceIfNeededAsync(resourceId);

            ResourceServiceModel result = null;

            if (resourceStatus == ResourceServiceStatus.Downloaded)
            {
                result = await LoadResourceAsync(resourceId);
            }
            else
            {
                ThrowInvalidResourceException(resourceId);
            }

            return result;
        }


        public async Task<ResourceServiceStatus> GetResourceStatusAsync(Guid resourceId)
        {
            var downloadedResourceAvailable = await _resourcesManager.CheckAvailability(resourceId);

            if (!downloadedResourceAvailable)
            {
                ResourceDetailsRequest.Initialize(resourceId);

                var response = await ResourceDetailsRequest.SendRequestAsync();

                if (response.NotFound())
                {
                    return ResourceServiceStatus.NotFound;
                }

                if (response.Succeeded)
                {
                    return ResourceServiceStatus.Available;
                }

                return ResourceServiceStatus.Unknown;
            }

            return ResourceServiceStatus.Downloaded;
        }

        private async Task<ResourceServiceModel> LoadResourceAsync(Guid resourceId)
        {
            var resourceDetails = await _resourcesManager.GetAsync(resourceId);
            var resourceStream = await _resourcesManager.LoadAsync(resourceId);

            if (resourceDetails.IsNotNull()
                && resourceStream.IsNotNull())
            {
                return new ResourceServiceModel()
                {
                    Name = resourceDetails.Name,
                    Resource = resourceStream,
                };
            }

            return null;
        }

        private void ThrowInvalidResourceException(Guid resourceId)
        {
            throw new InvalidOperationException($"Could not download resource {resourceId}");
        }
    }
}
