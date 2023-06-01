using AbobusMobile.AndroidRoot.Configurations;
using AbobusMobile.AndroidRoot.DataExchangeService;
using AbobusMobile.AndroidRoot.Extensions;
using AbobusMobile.BLL.Services.Abstractions.Accounts;
using AbobusMobile.BLL.Services.Abstractions.Authorization;
using AbobusMobile.BLL.Services.Abstractions.Comments;
using AbobusMobile.BLL.Services.Abstractions.Error;
using AbobusMobile.BLL.Services.Abstractions.Monuments;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.BLL.Services.Abstractions.Routes;
using AbobusMobile.BLL.Services.Abstractions.Utilities;
using AbobusMobile.BLL.Services.Accounts;
using AbobusMobile.BLL.Services.Authorization;
using AbobusMobile.BLL.Services.Comments;
using AbobusMobile.BLL.Services.Error;
using AbobusMobile.BLL.Services.Monuments;
using AbobusMobile.BLL.Services.Resources;
using AbobusMobile.BLL.Services.Routes;
using AbobusMobile.BLL.Services.Utitlities;
using AbobusMobile.DAL.Services.Abstractions.Accounts;
using AbobusMobile.DAL.Services.Abstractions.Authorization;
using AbobusMobile.DAL.Services.Abstractions.Comments;
using AbobusMobile.DAL.Services.Abstractions.Configurations;
using AbobusMobile.DAL.Services.Abstractions.Monuments;
using AbobusMobile.DAL.Services.Abstractions.Resources;
using AbobusMobile.DAL.Services.Abstractions.Routes;
using AbobusMobile.DAL.Services.Abstractions.Utilities;
using AbobusMobile.DAL.Services.Accounts;
using AbobusMobile.DAL.Services.Authorization;
using AbobusMobile.DAL.Services.Comments;
using AbobusMobile.DAL.Services.Configurations;
using AbobusMobile.DAL.Services.Monuments;
using AbobusMobile.DAL.Services.Resources;
using AbobusMobile.DAL.Services.Routes;
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

            //File.Delete(Path.Combine(
            //        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            //        configuration.GetConnectionString("DatabasePath")));

            //Directory.Delete(Path.Combine(
            //        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            //        configuration.GetConnectionString("ResourcesPath")),
            //        true);

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
            serviceCollection.AddSingleton<IAccountsDataManager, AccountsDataManager>();
            serviceCollection.AddSingleton<IResourcesDataManager, ResourcesDataManager>();
            serviceCollection.AddSingleton<ILocationsDataManager, LocationsDataManager>();
            serviceCollection.AddSingleton<IRoutesDataManager, RoutesDataManager>();
            serviceCollection.AddSingleton<ICommentsDataManager, CommentsDataManager>();
            serviceCollection.AddSingleton<IMonumentsDataManager, MonumentsDataManager>();

            // BLL
            serviceCollection.AddSingleton<IAuthorizationService, AuthorizationService>();
            serviceCollection.AddSingleton<IAccountsService, AccountsService>();
            serviceCollection.AddSingleton<IResourcesService, ResourcesService>();
            serviceCollection.AddSingleton<IErrorHandlingService, ErrorHandlingService>();
            serviceCollection.AddSingleton<ILocationService, LocationService>();
            serviceCollection.AddSingleton<IRouteService, RouteService>();
            serviceCollection.AddSingleton<IMonumentsService, MonumentsService>();
            serviceCollection.AddSingleton<ICommentsService, CommentsService>();

            // PL
            serviceCollection.AddSingleton<ExchangeService>();

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

        private RouteDetailsViewModel routeDetailsViewModel = null;
        public RouteDetailsViewModel RouteDetailsViewModel => routeDetailsViewModel ?? (routeDetailsViewModel = _provider.GetRequiredService<RouteDetailsViewModel>());

        private MonumentDetailsViewModel monumentDetailsViewModel = null;
        public MonumentDetailsViewModel MonumentDetailsViewModel => monumentDetailsViewModel ?? (monumentDetailsViewModel = _provider.GetRequiredService<MonumentDetailsViewModel>());
    }
}
