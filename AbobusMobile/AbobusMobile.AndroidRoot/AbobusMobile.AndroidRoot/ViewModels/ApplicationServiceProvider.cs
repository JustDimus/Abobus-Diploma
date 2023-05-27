using AbobusMobile.AndroidRoot.Extensions;
using AbobusMobile.Communication.Services;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Handlers;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.SQLite;
using Microsoft.Extensions.Configuration;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class ApplicationServiceProvider
    {
        private readonly TinyIoCContainer _container;

        public ApplicationServiceProvider()
        {
            Stream configurationStream = GetType().Assembly.GetManifestResourceStream(
                GetType().Assembly.GetName().Name + ".appsettings.json");

            var configuration = new ConfigurationBuilder()
                .AddJsonStream(configurationStream)
                .Build();

            _container = new TinyIoCContainer();

            _container
                .ConfigureEndpoints()
                .ConfigureRequestFactory()
                .ConfigureRequestHandlers();

            _container.AddDatabase(true, options =>
            {
                options.LoadTablesFromAssembly(typeof(ConfigurationModel).Assembly);
                options.UseDatabasePath(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    configuration.GetConnectionString("DatabasePath")));
            });
        }
    }
}
