using AbobusMobile.AndroidRoot.Configurations;
using AbobusMobile.AndroidRoot.ViewModels;
using AbobusMobile.Communication.Requests.Session;
using AbobusMobile.Communication.Services;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Abstractions.Configuration;
using AbobusMobile.Communication.Services.Abstractions.Handlers;
using AbobusMobile.Communication.Services.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbobusMobile.AndroidRoot.Extensions
{
    public static class IoCContainerExtensions
    {
        public static IServiceCollection AddOptions<TEntity>(
            this IServiceCollection services,
            string entity)
        {
            services
                .AddSingleton<Options<TEntity>>(new Options<TEntity>(entity));

            return services;
        }

        public static IServiceCollection AddRequestConsumerService(
            this IServiceCollection services,
            Action<IServiceProvider, RequestConsumerServiceConfiguration> configurator)
        {
            services.AddTransient<IRequestConsumerService, RequestConsumerService>();
            services.AddSingleton<RequestConsumerServiceConfiguration>(provider =>
            {
                var configuration = new RequestConsumerServiceConfiguration();

                configurator.Invoke(provider, configuration);

                return configuration;
            });

            return services;
        }

        public static IServiceCollection ConfigureRequestFactory(this IServiceCollection services)
        {
            var configuration = new RequestFactoryConfiguration();

            configuration.LoadRequestsFromAssembly(typeof(RequestFactoryConfiguration).Assembly);
            configuration.LoadRequestsFromAssembly(typeof(IoCContainerExtensions).Assembly);
            configuration.LoadRequestsFromAssembly(typeof(LoginRequest).Assembly);

            services.AddSingleton<RequestFactoryConfiguration>(configuration);

            services.AddSingleton<IRequestFactory, RequestFactory>();

            return services;
        }

        public static IServiceCollection ConfigureRequestHandlers(this IServiceCollection services)
        {
            services.AddSingleton<AuthorizatonHandler>();
            services.AddSingleton<IAuthorizationHandler>(provider => provider.GetRequiredService<AuthorizatonHandler>());

            services.AddSingleton<IRequestHandler>(provider => provider.GetRequiredService<AuthorizatonHandler>());
            services.AddSingleton<IRequestHandler, ExceptionHandler>();

            return services;
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            IEnumerable<Type> viewModels = typeof(IoCContainerExtensions).Assembly
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(BaseViewModel)));

            foreach (Type viewModel in viewModels)
            {
                services.AddSingleton(viewModel);
            }

            return services;
        }
    }
}
