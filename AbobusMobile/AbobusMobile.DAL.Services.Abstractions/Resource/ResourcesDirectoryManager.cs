using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.DAL.Services.Abstractions.Resource
{
    public class ResourcesDirectoryManager
    {
        private string ResourceFolderPath { get; set; }

        public ResourcesDirectoryManager UsePath(string resourcesDirectoryPath)
        {
            ResourceFolderPath = resourcesDirectoryPath;

            return this;
        }
    }
}
