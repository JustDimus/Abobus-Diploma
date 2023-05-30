using AbobusMobile.DAL.Services.Abstractions.Resource;
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

        public ResourcesDataManager(
            IRepository<ResourceModel> resources) 
        {
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public Task CreateAsync(CreateResourceDataModel resource)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid resourceId)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceDataModel> GetAsync(Guid resourceId)
        {
            throw new NotImplementedException();
        }

        public Task<MemoryStream> LoadAsync(Guid resourceId)
        {
            throw new NotImplementedException();
        }
    }
}
