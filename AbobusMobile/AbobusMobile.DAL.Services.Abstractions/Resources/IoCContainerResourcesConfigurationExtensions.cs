using Microsoft.Extensions.DependencyInjection;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.DAL.Services.Abstractions.Resources
{
    public static class IoCContainerResourcesConfigurationExtensions
    {
        public static IServiceCollection ConfigureResources(
            this IServiceCollection container,
            Action<ResourcesDirectoryManager> options)
        {
            var resourceManager = new ResourcesDirectoryManager();

            options?.Invoke(resourceManager);

            container.AddSingleton<ResourcesDirectoryManager>(resourceManager);

            return container;
        }
    }
}
