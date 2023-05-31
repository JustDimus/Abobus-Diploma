using AbobusMobile.AndroidRoot.Configurations;
using AbobusMobile.AndroidRoot.Extensions;
using AbobusMobile.BLL.Services.Abstractions.Account;
using AbobusMobile.BLL.Services.Abstractions.Authorization;
using AbobusMobile.BLL.Services.Abstractions.Error;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.BLL.Services.Abstractions.Routes;
using AbobusMobile.BLL.Services.Abstractions.Utilities;
using AbobusMobile.BLL.Services.Account;
using AbobusMobile.BLL.Services.Authorization;
using AbobusMobile.BLL.Services.Error;
using AbobusMobile.BLL.Services.Resources;
using AbobusMobile.BLL.Services.Routes;
using AbobusMobile.BLL.Services.Utitlities;
using AbobusMobile.DAL.Services.Abstractions.Account;
using AbobusMobile.DAL.Services.Abstractions.Authorization;
using AbobusMobile.DAL.Services.Abstractions.Configurations;
using AbobusMobile.DAL.Services.Abstractions.Resource;
using AbobusMobile.DAL.Services.Abstractions.Utilities;
using AbobusMobile.DAL.Services.Account;
using AbobusMobile.DAL.Services.Authorization;
using AbobusMobile.DAL.Services.Configurations;
using AbobusMobile.DAL.Services.Resources;
using AbobusMobile.DAL.Services.Utilities;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.SQLite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class ApplicationServiceProvider
    {
        private readonly IServiceProvider _provider;

        public ApplicationServiceProvider()
        {
            Stream configurationStream = GetType().Assembly.GetManifestResourceStream(
                GetType().Assembly.GetName().Name + ".appsettings.json");

            var configuration = new ConfigurationBuilder()
                .AddJsonStream(configurationStream)
                .Build();

            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection
                .AddOptions<EndpointConfigurations>(GetConfigurationString(nameof(EndpointConfigurations)));

            serviceCollection
                .AddRequestConsumerService((provider, options) =>
                {
                    var endpoints = provider.GetRequiredService<Options<EndpointConfigurations>>();

                    options.UseRelativeUrls = true;
                    options.BaseURL = endpoints.Value.ApiUrl;
                })
                .ConfigureRequestFactory()
                .ConfigureRequestHandlers();

            serviceCollection.AddDatabase(true, options =>
            {
                options.LoadTablesFromAssembly(typeof(ConfigurationModel).Assembly);
                options.UseDatabasePath(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    configuration.GetConnectionString("DatabasePath")));
            });

            serviceCollection.AddViewModels();

            // DAL
            serviceCollection.ConfigureResources(options =>
            {
                options.UsePath(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    configuration.GetConnectionString("ResourcesPath")));
            });

            serviceCollection.AddSingleton<IAuthorizationDataManager, AuthorizationDataManager>();
            serviceCollection.AddSingleton<IConfigurationsDataManager, ConfigurationsDataManager>();
            serviceCollection.AddSingleton<IAccountDataManager, AccountDataManager>();
            serviceCollection.AddSingleton<IResourcesDataManager, ResourcesDataManager>();
            serviceCollection.AddSingleton<ILocationsDataManager, LocationsDataManager>();

            // BLL
            serviceCollection.AddSingleton<IAuthorizationService, AuthorizationService>();
            serviceCollection.AddSingleton<IAccountService, AccountService>();
            serviceCollection.AddSingleton<IResourcesService, ResourcesService>();
            serviceCollection.AddSingleton<IErrorHandlingService, ErrorHandlingService>();
            serviceCollection.AddSingleton<ILocationService, LocationService>();
            serviceCollection.AddSingleton<IRouteService, RouteService>();

            _provider = serviceCollection.BuildServiceProvider();

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
        public AppShellViewModel AppShellViewModel => appShellViewModel ?? (appShellViewModel = _provider.GetRequiredService<AppShellViewModel>());

        private LoginViewModel loginViewModel = null;
        public LoginViewModel LoginViewModel => loginViewModel ?? (loginViewModel = _provider.GetRequiredService<LoginViewModel>());

        private StartViewModel startViewModel = null;
        public StartViewModel StartViewModel => startViewModel ?? (startViewModel = _provider.GetRequiredService<StartViewModel>());

        private ProfileViewModel profileViewModel = null;
        public ProfileViewModel ProfileViewModel => profileViewModel ?? (profileViewModel = _provider.GetRequiredService<ProfileViewModel>());

        private RoutesViewModel routesViewModel = null;
        public RoutesViewModel RoutesViewModel => routesViewModel ?? (routesViewModel = _provider.GetRequiredService<RoutesViewModel>());
    }
}
