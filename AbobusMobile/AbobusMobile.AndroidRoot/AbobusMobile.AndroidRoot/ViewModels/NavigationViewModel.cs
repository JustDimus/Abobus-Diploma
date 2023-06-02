using AbobusMobile.AndroidRoot.Services;
using AbobusMobile.BLL.Services.Abstractions.Routes;
using AbobusMobile.BLL.Services.Abstractions.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
        public class PinModel
        {
            public Position Position { get; set; }

            public PinType PinType { get; set; }

            public string Label { get; set; }
        }

        public class PathModel
        {
            public List<Position> PathPositions { get; set; }
        }

        public class NavigationModel
        {
            public bool IsPin { get; set; }

            public PinModel Pin { get; set; }

            public bool IsPath { get; set; }
        }

        private readonly ExchangeService _exchangeService;
        private IRouteService _routeService;
        private ILocationService _locationService;

        private IDisposable exchangeDisposable;

        public NavigationViewModel(
            ExchangeService exchangeService,
            IRouteService routeService,
            ILocationService locationService)
        {
            _exchangeService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));

            exchangeDisposable = exchangeService.OnRouteStartRequested
                .Subscribe(OnRouteSelected);
        }

        private Guid? selectedRouteId;

        public MapManipulatorService MapService { get; set; }

        #region Properties

        private bool updateRequired;
        public bool UpdateRequired
        {
            get => updateRequired;
            set => SetProperty(ref updateRequired, value);
        }

        private bool routeStarted;
        public bool RouteStarted
        {
            get => routeStarted;
            set => SetProperty(ref routeStarted, value);
        }

        public bool ShowRoute
        {
            get => RouteStarted && !UpdateRequired;
        }

        public bool ShowUpdateRequired
        {
            get => RouteStarted && UpdateRequired;
        }

        public bool ShowRouteRequired
        {
            get => !RouteStarted;
        }

        #endregion
        protected override async void OnPageAppeared()
        {
            if ((UpdateRequired
                && RouteStarted
                && MapService != null)
                || true)
            {


                await UpdatePageAsync();
            }
        }

        private async Task UpdatePageAsync()
        {
            UpdateRequired = true;

            await Task.Delay(2000);

            var coordinates = await _locationService.GetCurrentLocationCoordinatesAsync();

            MapService.AddPin(coordinates.Longitude, coordinates.Latitude);

            UpdateRequired = false;
            OnShowParametersChanged();
        }

        private void OnRouteSelected(Guid routeId)
        {
            if (!RouteStarted)
            {
                selectedRouteId = routeId;
                RouteStarted = true;
                UpdateRequired = true;
            }

            OnShowParametersChanged();
        }

        private void OnShowParametersChanged()
        {
            OnPropertyChanged(nameof(ShowRoute));
            OnPropertyChanged(nameof(ShowUpdateRequired));
            OnPropertyChanged(nameof(ShowRouteRequired));
        }
    }
}
