using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace AbobusMobile.AndroidRoot.Services
{
    public class ExchangeService
    {
        private ReplaySubject<Guid> routeRequestedSubject = new ReplaySubject<Guid>();
        private ReplaySubject<Guid> routeCancelledSubject = new ReplaySubject<Guid>();

        private ReplaySubject<Guid> monumentRequestedSubject = new ReplaySubject<Guid>();

        private ReplaySubject<Guid> routeStartRequestedSubject = new ReplaySubject<Guid>();

        public IObservable<Guid> OnRouteRequested => routeRequestedSubject.AsObservable();
        public IObservable<Guid> OnRouteCancelled => routeCancelledSubject.AsObservable();

        public IObservable<Guid> OnMonumentRequested => monumentRequestedSubject.AsObservable();

        public IObservable<Guid> OnRouteStartRequested => routeStartRequestedSubject.AsObservable();

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

        public void RequestRouteStart(Guid routeId)
        {
            routeStartRequestedSubject.OnNext(routeId);
        }
    }
}
