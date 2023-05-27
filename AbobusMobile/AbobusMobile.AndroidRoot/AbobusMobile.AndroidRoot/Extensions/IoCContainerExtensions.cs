using AbobusMobile.Communication.Services;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Abstractions.Configuration;
using AbobusMobile.Communication.Services.Handlers;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.AndroidRoot.Extensions
{
    public static class IoCContainerExtensions
    {
        public static TinyIoCContainer ConfigureEndpoints(this TinyIoCContainer container)
        {
            container.Register<RequestConsumerServiceConfiguration>(new RequestConsumerServiceConfiguration()
            {
                UseRelativeUrls = true,
                BaseURL = @"http://localhost:5555/api/v1/"
            });

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

            return container;
        }
    }
}
