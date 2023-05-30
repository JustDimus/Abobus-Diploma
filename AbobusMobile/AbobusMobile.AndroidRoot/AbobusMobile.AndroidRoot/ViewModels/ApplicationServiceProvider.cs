using AbobusMobile.AndroidRoot.Configurations;
using AbobusMobile.AndroidRoot.Extensions;
using AbobusMobile.BLL.Services.Abstractions.Account;
using AbobusMobile.BLL.Services.Abstractions.Authorization;
using AbobusMobile.BLL.Services.Abstractions.Error;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.BLL.Services.Account;
using AbobusMobile.BLL.Services.Authorization;
using AbobusMobile.BLL.Services.Error;
using AbobusMobile.BLL.Services.Resources;
using AbobusMobile.Communication.Services;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Abstractions.Configuration;
using AbobusMobile.Communication.Services.Handlers;
using AbobusMobile.DAL.Services.Abstractions.Account;
using AbobusMobile.DAL.Services.Abstractions.Authorization;
using AbobusMobile.DAL.Services.Abstractions.Configurations;
using AbobusMobile.DAL.Services.Abstractions.Resource;
using AbobusMobile.DAL.Services.Account;
using AbobusMobile.DAL.Services.Authorization;
using AbobusMobile.DAL.Services.Configurations;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.SQLite;
using Microsoft.Extensions.Configuration;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                .AddOptions<EndpointConfigurations>(GetConfigurationString(nameof(EndpointConfigurations)));

            _container
                .AddRequestConsumerService(options =>
                {
                    var endpoints = _container.Resolve<Options<EndpointConfigurations>>();

                    options.UseRelativeUrls = true;
                    options.BaseURL = endpoints.Value.ApiUrl;
                })
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

            // DAL
            _container.ConfigureResources(options =>
            {
                options.UsePath(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    configuration.GetConnectionString("ResourcesPath")));
            });

            _container.Register<IAuthorizationDataManager, AuthorizationDataManager>();
            _container.Register<IConfigurationsDataManager, ConfigurationsDataManager>();
            _container.Register<IAccountDataManager, AccountDataManager>();

            // BLL
            _container.Register<IAuthorizationService, AuthorizationService>();
            _container.Register<IAccountService, AccountService>();
            _container.Register<IResourcesService, ResourcesService>();
            _container.Register<IErrorHandlingService, ErrorHandlingService>();

            string GetConfigurationString(string sectionName)
            {
                var sectionItems = configuration
                    .GetSection(sectionName)
                    .GetChildren().SelectMany(i => i.AsEnumerable()).ToList();

                StringBuilder builder = new StringBuilder();

                builder.Append("{");
                builder.Append(string.Join(",", sectionItems.Select(i => $"\"{i.Key.Split(':')[1]}\":\"{i.Value}\"")));
                builder.Append("}");

                return builder.ToString();
            }
        }

        private AppShellViewModel appShellViewModel = null;
        public AppShellViewModel AppShellViewModel => appShellViewModel ?? (appShellViewModel = _container.Resolve<AppShellViewModel>());

        private LoginViewModel loginViewModel = null;
        public LoginViewModel LoginViewModel => loginViewModel ?? (loginViewModel = _container.Resolve<LoginViewModel>());

        private StartViewModel startViewModel = null;
        public StartViewModel StartViewModel => startViewModel ?? (startViewModel = _container.Resolve<StartViewModel>());

        private ProfileViewModel profileViewModel = null;
        public ProfileViewModel ProfileViewModel => profileViewModel ?? (profileViewModel = _container.Resolve<ProfileViewModel>());
    }
}
