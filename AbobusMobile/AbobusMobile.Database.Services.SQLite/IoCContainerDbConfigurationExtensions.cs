using AbobusMobile.Database.Services.Abstractions;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Services.SQLite
{
    public static class IoCContainerDbConfigurationExtensions
    {
        public static TinyIoCContainer AddDatabase(
            this TinyIoCContainer container,
            bool asSingleton = false,
            Action<DbOptions> optionsAction = null)
        {
            var options = container.Register<DbContext>();

            if (asSingleton)
            {
                options.AsSingleton();
            }

            var configuration = new DbOptions();

            optionsAction?.Invoke(configuration);

            container.Register(typeof(IRepository<>), typeof(Repository<>));

            container.Register<DbOptions>(configuration).AsSingleton();

            return container;
        }
    }
}
