using AbobusMobile.Communication.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AbobusMobile.Communication.Services.Abstractions.Configuration
{
    public class RequestFactoryConfiguration
    {
        private readonly List<Type> _loadedRequests = new List<Type>();

        public IEnumerable<Type> AvailableRequests => _loadedRequests;

        public void LoadRequestsFromAssembly(Assembly assembly)
        {
            var requestsTypes = assembly.GetTypes()
                .OfType<BaseRequest>()
                .Where(request => !request.GetType().IsAbstract)
                .Select(request => request.GetType());

            foreach (var requestType in requestsTypes)
            {
                if (!_loadedRequests.Contains(requestType))
                {
                    _loadedRequests.Add(requestType);
                }
            }
        }
    }
}
