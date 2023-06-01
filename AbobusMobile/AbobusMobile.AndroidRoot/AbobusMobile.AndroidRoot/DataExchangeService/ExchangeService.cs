using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace AbobusMobile.AndroidRoot.DataExchangeService
{
    public class ExchangeService
    {
        private ReplaySubject<Guid> routeRequestedSubject = new ReplaySubject<Guid>();
        private ReplaySubject<Guid> routeCancelledSubject = new ReplaySubject<Guid>();

        private ReplaySubject<Guid> monumentRequestedSubject = new ReplaySubject<Guid>();

        public IObservable<Guid> OnRouteRequested => routeRequestedSubject.AsObservable();
        public IObservable<Guid> OnRouteCancelled => routeCancelledSubject.AsObservable();

        public IObservable<Guid> OnMonumentRequested => monumentRequestedSubject.AsObservable();

        public void RequestMonument(Guid monumentId)
        {
            monumentRequestedSubject.OnNext(monumentId);
        }

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
