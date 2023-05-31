using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Abstractions.Resources
{
    public class ResourcesDirectoryManager
    {
        private string ResourceFolderPath { get; set; }

        public ResourcesDirectoryManager UsePath(string resourcesDirectoryPath)
        {
            ResourceFolderPath = resourcesDirectoryPath;

            if (!Directory.Exists(resourcesDirectoryPath))
            {
                Directory.CreateDirectory(resourcesDirectoryPath);
            }

            return this;
        }

        public Task<bool> CheckResourceFileAvailabilityAsync(string resourcePath)
        {
            return Task.Run(() => GetResourceFile(resourcePath).Exists);
        }

        public async Task WriteResourceFileAsync(string resourcePath, Stream sourceStream)
        {
            var resource = GetResourceFile(resourcePath);

            using (var writeStream = resource.Create())
            {
                sourceStream.Seek(0, SeekOrigin.Begin);
                await sourceStream.CopyToAsync(writeStream);
            }
        }

        public Task DeleteResourceFileAsync(string resourcePath)
        {
            return Task.Run(() => GetResourceFile(resourcePath).Delete());
        }

        public async Task<MemoryStream> LoadResourceFileAsync(string resourcePath)
        {
            var resource = GetResourceFile(resourcePath);

            var resultStream = new MemoryStream();

            using (var readStream = resource.OpenRead())
            {
                await readStream.CopyToAsync(resultStream);
            }

            resultStream.Seek(0, SeekOrigin.Begin);
            return resultStream;
        }

        private FileInfo GetResourceFile(string resourcePath)
            => new FileInfo(GetAbsolutePath(resourcePath));

        private string GetAbsolutePath(string resourcePath)
            => Path.Combine(ResourceFolderPath, resourcePath);
    }
}
