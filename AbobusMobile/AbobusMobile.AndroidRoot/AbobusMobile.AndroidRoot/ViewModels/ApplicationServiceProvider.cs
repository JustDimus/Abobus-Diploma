using AbobusMobile.AndroidRoot.Extensions;
using AbobusMobile.BLL.Services.Abstractions.Authorization;
using AbobusMobile.BLL.Services.Authorization;
using AbobusMobile.Communication.Services;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Handlers;
using AbobusMobile.DAL.Services.Abstractions.Authorization;
using AbobusMobile.DAL.Services.Authorization;
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

            _container.AddViewModels();

            _container.Register<IAuthorizationDataManager, AuthorizationDataManager>();
            _container.Register<IAuthorizationService, AuthorizationService>();
        }

        private AppShellViewModel appShellViewModel = null;
        public AppShellViewModel AppShellViewModel => appShellViewModel ?? (appShellViewModel = _container.Resolve<AppShellViewModel>());

        private LoginViewModel loginViewModel = null;
        public LoginViewModel LoginViewModel => loginViewModel ?? (loginViewModel = _container.Resolve<LoginViewModel>());

        private StartViewModel startViewModel = null;
        public StartViewModel StartViewModel => startViewModel ?? (startViewModel = _container.Resolve<StartViewModel>());
    }
}
