using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace AbobusMobile.AndroidRoot.DataExchangeService
{
    public class RouteExchangeService
    {
        private ReplaySubject<Guid> routeRequestedSubject = new ReplaySubject<Guid>();
        private ReplaySubject<Guid> routeCancelledSubject = new ReplaySubject<Guid>();

        public IObservable<Guid> OnRouteRequested => routeRequestedSubject.AsObservable();
        public IObservable<Guid> OnRouteCancelled => routeCancelledSubject.AsObservable();

        public void RequestRoute(Guid routeId)
        {
            routeRequestedSubject.OnNext(routeId);
        }

        public void CancelRoute(Guid routeId)
        {
            routeCancelledSubject.OnNext(routeId);
        }
    }
}
