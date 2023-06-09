﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Abstractions.Resources
{
    public interface IResourcesDataManager
    {
        Task<bool> CheckAvailability(Guid resourceId);

        Task<ResourceDataModel> GetAsync(Guid resourceId);

        Task<MemoryStream> LoadAsync(Guid resourceId);

        Task DeleteAsync(Guid resourceId);

        Task CreateAsync(CreateResourceDataModel resource);
    }
}
