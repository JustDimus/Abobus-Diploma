using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.DAL.Services.Abstractions.Resource
{
    public static class IoCContainerResourcesConfigurationExtensions
    {
        public static TinyIoCContainer ConfigureResources(
            this TinyIoCContainer container,
            Action<ResourcesDirectoryManager> options)
        {
            var resourceManager = new ResourcesDirectoryManager();

            options?.Invoke(resourceManager);

            container.Register<ResourcesDirectoryManager>(resourceManager);

            return container;
        }
    }
}
