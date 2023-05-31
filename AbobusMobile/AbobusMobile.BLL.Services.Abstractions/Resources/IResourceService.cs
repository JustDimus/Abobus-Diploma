using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Abstractions.Resources
{
    public interface IResourcesService
    {
        Task<ResourceServiceStatus> GetResourceStatusAsync(Guid resourceId);

        Task<ResourceServiceStatus> DownloadResourceAsync(Guid resourceId);

        Task<ResourceServiceStatus> DownloadeResourceIfNeededAsync(Guid resourceId);

        Task<ResourceServiceModel> GetResourceAsync(Guid resourceId);

        Task<ResourceServiceStatus> DeleteResourceAsync(Guid resourceId);
    }
}
