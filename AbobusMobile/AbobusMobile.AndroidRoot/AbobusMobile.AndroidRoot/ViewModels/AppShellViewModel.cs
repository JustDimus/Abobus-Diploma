using AbobusMobile.AndroidRoot.DataExchangeService;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {
        private readonly ExchangeService _routeExchangeService;

        private IDisposable routeExchangeDisposable;

        public AppShellViewModel(
            ExchangeService routeExchangeService)
        {
            _routeExchangeService = routeExchangeService ?? throw new ArgumentNullException(nameof(routeExchangeService));

            routeExchangeDisposable = _routeExchangeService.OnRouteRequested
                .Subscribe((_) => ShowRouteDetailsTab = true);
        }

        private bool showRouteDetailsTab = false;
        public bool ShowRouteDetailsTab
        {
            get => showRouteDetailsTab;
            set => SetProperty(ref showRouteDetailsTab, value);
        }
    }
}
