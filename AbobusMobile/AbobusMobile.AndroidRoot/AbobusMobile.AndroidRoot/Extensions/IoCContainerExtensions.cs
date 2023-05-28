using AbobusMobile.AndroidRoot.ViewModels;
using AbobusMobile.Communication.Services;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Abstractions.Configuration;
using AbobusMobile.Communication.Services.Abstractions.Handlers;
using AbobusMobile.Communication.Services.Handlers;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbobusMobile.AndroidRoot.Extensions
{
    public static class IoCContainerExtensions
    {
        public static TinyIoCContainer AddRequestConsumerService(
            this TinyIoCContainer container,
            Action<RequestConsumerServiceConfiguration> configurator)
        {
            var configuration = new RequestConsumerServiceConfiguration();

            configurator.Invoke(configuration);

            container.Register<IRequestConsumerService, RequestConsumerService>();
            container.Register<RequestConsumerServiceConfiguration>(configuration);

            return container;
        }

        public static TinyIoCContainer ConfigureRequestFactory(this TinyIoCContainer container)
        {
            var configuration = new RequestFactoryConfiguration();

            configuration.LoadRequestsFromAssembly(typeof(RequestFactoryConfiguration).Assembly);
            configuration.LoadRequestsFromAssembly(typeof(IoCContainerExtensions).Assembly);

            container.Register<RequestFactoryConfiguration>(configuration);

            container.Register<IRequestFactory, RequestFactory>().AsSingleton();

            return container;
        }

        public static TinyIoCContainer ConfigureRequestHandlers(this TinyIoCContainer container)
        {
            container.RegisterMultiple<IRequestHandler>(new Type[]
            {
                typeof(AuthorizatonHandler),
                typeof(ExceptionHandler),
            }).AsSingleton();

            container.Register<IAuthorizationHandler>((provider, _) => provider.Resolve<AuthorizatonHandler>());

            return container;
        }

        public static TinyIoCContainer AddViewModels(this TinyIoCContainer container)
        {
            IEnumerable<Type> viewModels = typeof(IoCContainerExtensions).Assembly
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(BaseViewModel)));

            foreach (Type viewModel in viewModels)
            {
                container.Register(viewModel);
            }

            return container;
        }
    }
}
