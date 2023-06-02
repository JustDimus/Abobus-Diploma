using AbobusMobile.AndroidRoot.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {
        private readonly ExchangeService _exchangeService;

        private IDisposable routeExchangeDisposable;
        private IDisposable monumentExchangeDisposable;

        public AppShellViewModel(
            ExchangeService exchangeService)
        {
            _exchangeService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));

            routeExchangeDisposable = _exchangeService.OnRouteRequested
                .Subscribe((_) => ShowRouteDetailsTab = true);

            monumentExchangeDisposable = _exchangeService.OnMonumentRequested
                .Subscribe((_) =>
                {
                    ShowMonumentDetailsTab = true;
                });
        }

        private bool showRouteDetailsTab = false;
        public bool ShowRouteDetailsTab
        {
            get => showRouteDetailsTab;
            set => SetProperty(ref showRouteDetailsTab, value);
        }

        private bool showMonumentDetailsTab = false;
        public bool ShowMonumentDetailsTab
        {
            get => showMonumentDetailsTab;
            set => SetProperty(ref showMonumentDetailsTab, value);
        }
    }
}
