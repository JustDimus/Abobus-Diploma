using AbobusMobile.Database.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Services.SQLite
{
    public static class IoCContainerDbConfigurationExtensions
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection container,
            bool asSingleton = false,
            Action<DbOptions> optionsAction = null)
        {
            if (asSingleton)
            {
                container.AddSingleton<DbContext>();
            }
            else
            {
                container.AddTransient<DbContext>();
            }

            var configuration = new DbOptions();

            optionsAction?.Invoke(configuration);

            container.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            container.AddSingleton<DbOptions>(configuration);

            return container;
        }
    }
}
