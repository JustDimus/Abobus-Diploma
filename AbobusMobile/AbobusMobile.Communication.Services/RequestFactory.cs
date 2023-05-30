using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.Communication.Services.Abstractions.Configuration;
using AbobusMobile.Communication.Services.Abstractions.Models;
using AbobusMobile.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbobusMobile.Communication.Services
{
    public class RequestFactory : IRequestFactory, IRequestSender
    {
        private class RequestParameters
        {
            public ConstructorInfo Constructor { get; set; }

            public MethodInfo SetupSenderMethod { get; set; }

            public MethodInfo SetupResponseMethod { get; set; }
        }

        private readonly IRequestConsumerService _requestConsumerService;

        private readonly List<IRequestHandler> _handlers;
        private readonly List<Type> _registeredRequestTypes;
        private readonly Dictionary<Type, RequestParameters> _cachedTypes = new Dictionary<Type, RequestParameters>();

        public RequestFactory(
            IEnumerable<IRequestHandler> requestHandlers,
            RequestFactoryConfiguration factoryConfiguration,
            IRequestConsumerService requestConsumerService)
        {
            var requestHanle = requestHandlers.ToList();
            _requestConsumerService = requestConsumerService ?? throw new ArgumentNullException(nameof(requestConsumerService));

            _handlers = requestHandlers?.ToList() ?? throw new ArgumentNullException(nameof(requestHandlers));

            foreach (var handler in _handlers)
            {
                handler.SetupRequestFactory(this);
            }

            _registeredRequestTypes = factoryConfiguration?.AvailableRequests?.ToList() ?? throw new ArgumentNullException(nameof(_registeredRequestTypes));
        }

        public TRequest CreateRequest<TRequest>() where TRequest : BaseRequest, new()
            => (TRequest)CreateRequest(typeof(TRequest));

        public BaseRequest CreateRequest(Type requestType)
        {
            if (!_cachedTypes.ContainsKey(requestType))
            {
                ExtractRequiredMethods(requestType);
            }

            var request = (BaseRequest)_cachedTypes[requestType].Constructor.Invoke(new object[] { });

            _cachedTypes[requestType].SetupSenderMethod.Invoke(request, new object[] { this });

            return request;
        }

        public async Task<BaseResponse> SendRequest(BaseRequest request)
        {
            foreach (var handler in _handlers)
            {
                await handler.HandleRequest(request);
            }

            request.ConfigureRequest();

            var response = await _requestConsumerService.SendRequestAsync(request);

            if (!_cachedTypes.ContainsKey(request.GetType()))
            {
                ExtractRequiredMethods(request.GetType());
            }

            _cachedTypes[request.GetType()].SetupResponseMethod.Invoke(request, new object[] { response });

            foreach (var handler in _handlers)
            {
                await handler.HandleResponse(request);
            }

            return request.Response;
        }

        private void ExtractRequiredMethods(Type requestType)
        {
            requestType.ValidateEmptyConstructor();
            requestType.ValidateBaseType(typeof(BaseRequest));
            requestType.ValidateTypeMethod(method
                => method.Name == CommunicationConstants.REQUEST_SENDER_SETUP_METHOD_NAME
                && method.IsFamily);
            requestType.ValidateTypeMethod(method
                => method.Name == CommunicationConstants.RESPONSE_SETUP_METHOD_NAME
                && method.IsFamily);

            _cachedTypes.Add(requestType, new RequestParameters
            {
                Constructor = requestType.GetConstructor(Type.EmptyTypes),
                SetupSenderMethod = requestType.GetMethod(
                    CommunicationConstants.REQUEST_SENDER_SETUP_METHOD_NAME,
                    BindingFlags.NonPublic | BindingFlags.Instance),
                SetupResponseMethod = requestType.GetMethod(
                    CommunicationConstants.RESPONSE_SETUP_METHOD_NAME,
                    BindingFlags.NonPublic | BindingFlags.Instance)
            });
        }
    }
}
