using AbobusMobile.AndroidRoot.Helpers;
using AbobusMobile.AndroidRoot.Models;
using AbobusMobile.AndroidRoot.Services;
using AbobusMobile.AndroidRoot.Views;
using AbobusMobile.BLL.Services.Abstractions.Monuments;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.BLL.Services.Abstractions.Routes;
using AbobusMobile.BLL.Services.Abstractions.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using static Android.Provider.DocumentsContract;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
        public class MonumentModel
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public ImageSource MonumentImage { get; set; }
        }

        private readonly ExchangeService _exchangeService;
        private IRouteService _routeService;
        private ILocationService _locationService;
        private IResourcesService _resourcesService;
        private IMonumentsService _monumentsService;

        private IDisposable exchangeDisposable;

        public NavigationViewModel(
            ExchangeService exchangeService,
            IRouteService routeService,
            IResourcesService resourcesService,
            ILocationService locationService,
            IMonumentsService monumentsService)
        {
            _exchangeService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
            _resourcesService = resourcesService ?? throw new ArgumentNullException(nameof(resourcesService));
            _monumentsService = monumentsService ?? throw new ArgumentNullException(nameof(monumentsService));

            exchangeDisposable = exchangeService.OnRouteStartRequested
                .Subscribe(OnRouteSelected);

            OpenMonumentDetailsCommand = new Command(async () => await OpenMonumentDetailsAsync(), () => RouteStarted);
            MoveNextCommand = new Command(async () => await MoveNextAsync(), () => !RouteCompleted && RouteStarted);
            InterruptRouteCommand = new Command(async () => await InterruptRouteAsync(), () => !RouteCompleted && RouteStarted);

            PropertyChanged += (_, __) =>
            {
                OpenMonumentDetailsCommand.ChangeCanExecute();
                MoveNextCommand.ChangeCanExecute();
            };
        }

        #region Commands
        public Command OpenMonumentDetailsCommand { get; }
        public Command MoveNextCommand { get; }
        public Command InterruptRouteCommand { get; }
        #endregion

        private Guid? selectedRouteId;
        private RouteDataModel currentRoute;
        private int currentRoutePosition;
        private int monumentsCount;
        private int pointsCount;

        private int currentPinId;
        private int nextPointPinId = -1;

        public int NewPinId => currentPinId++;

        private List<MonumentModel> routeMonuments = new List<MonumentModel>();

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

        private bool routeCompleted = false;
        public bool RouteCompleted
        {
            get => routeCompleted;
            set => SetProperty(ref routeCompleted, value);
        }

        private bool nextPointIsLast;
        public bool NextPointIsLast
        {
            get => nextPointIsLast;
            set => SetProperty(ref nextPointIsLast, value);
        }

        private MonumentModel nextMonument;
        public MonumentModel NextMonument
        {
            get => nextMonument;
            set => SetProperty(ref nextMonument, value);
        }

        private bool monumentArrived = false;
        public bool MonumentArrived
        {
            get => monumentArrived;
            set => SetProperty(ref monumentArrived, value);
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

            var route = await _routeService.GetRouteDetailsAsync(selectedRouteId.Value);
            var routeResource = await _resourcesService.GetResourceAsync(route.RouteResourceId);
            var routeMonuments = await _monumentsService.GetMonumentsByRouteIdAsync(route.Id);

            await UpdateRouteMonuments(routeMonuments);

            currentRoute = ConvertRouteData(routeResource.Resource);

            pointsCount = currentRoute.RoutePoints.Count;
            monumentsCount = currentRoute.RouteMonuments.Count;

            var firstMonumentPoint = currentRoute.RoutePoints.OrderBy(i => i.Order).First(i => i.IsDestination);

            SetNextMonument(firstMonumentPoint.MonumentId.Value);

            currentRoutePosition = -1;

            UpdateMap();
            await MoveNextAsync();

            NextPointIsLast = false;
            UpdateRequired = false;
            RouteCompleted = false;
        }

        private async Task InterruptRouteAsync()
        {
            currentRoutePosition = pointsCount;

            await MoveNextAsync();
        }

        private async Task MoveNextAsync()
        {
            currentRoutePosition++;

            if (currentRoutePosition >= pointsCount - 1)
            {
                RouteCompleted = true;

                if (nextPointPinId != -1)
                {
                    MapService.RemovePin(nextPointPinId);
                    nextPointPinId = -1;
                }

                return;
            }

            if (MonumentArrived)
            {
                currentRoutePosition--;
                MonumentArrived = false;
            }

            var currentPoint = currentRoute.RoutePoints[currentRoutePosition];

            if (currentPoint.IsDestination)
            {
                MonumentArrived = true;

                if (nextPointPinId != -1)
                {
                    MapService.RemovePin(nextPointPinId);
                    nextPointPinId = -1;
                }
            }
            else
            {
                if (nextPointPinId != -1)
                {
                    MapService.RemovePin(nextPointPinId);
                }

                var nextPoint = currentRoute.RoutePoints[currentRoutePosition];
                nextPointPinId = NewPinId;

                MapService.AddPin(new MapPinModel()
                {
                    Label = "Next",
                    Coordinate = new MapCoordinateModel() { Longitude = nextPoint.Coordinate.Longitude, Latitude = nextPoint.Coordinate.Latitude },
                    Id = nextPointPinId,
                    PinType = PinType.SearchResult,
                });

                if (currentRoutePosition == pointsCount - 2)
                {
                    NextPointIsLast = true;
                }
            }
        }

        private async Task OpenMonumentDetailsAsync()
        {
            _exchangeService.RequestMonument(NextMonument.Id);
            await Shell.Current.GoToAsync(PathConstants.MONUMENT_DETAILS_ABSOLUTE);
        }

        private void SetNextMonument(Guid monumentId)
        {
            NextMonument = routeMonuments.First(i => i.Id == monumentId);
        }

        private void UpdateMap()
        {
            MapService.ClearAll();

            foreach (var monument in currentRoute.RouteMonuments)
            {
                MapService.AddPin(new MapPinModel()
                {
                    Id = NewPinId,
                    Label = routeMonuments.First(i => i.Id == monument.MonumentId).Name,
                    PinType = PinType.SavedPin,
                    Coordinate = new MapCoordinateModel() { Longitude = monument.Coordinate.Longitude, Latitude = monument.Coordinate.Latitude }
                });
            }
        }

        private async Task UpdateRouteMonuments(List<MonumentServiceModel> monumentServiceModels)
        {
            routeMonuments.Clear();

            foreach (var monument in monumentServiceModels)
            {
                routeMonuments.Add(await ConvertMonumentModel(monument));
            }
        }

        private async Task<MonumentModel> ConvertMonumentModel(MonumentServiceModel serviceModel)
        {
            var monumentTitleImageResource = await _resourcesService.GetResourceAsync(serviceModel.MonumentTitleImageId);

            return new MonumentModel()
            {
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                MonumentImage = ImageHelper.Create(monumentTitleImageResource.Resource),
            };
        }

        private RouteDataModel ConvertRouteData(Stream source)
        {
            var serializer = JsonSerializer.Create();

            using (var streamReader = new StreamReader(source))
            {
                using (var reader = new JsonTextReader(streamReader))
                {
                    return JsonSerializer.CreateDefault().Deserialize<RouteDataModel>(reader);
                }
            }
        }

        private void OnRouteSelected(Guid routeId)
        {
            if (!RouteStarted
                || RouteCompleted)
            {
                selectedRouteId = routeId;
                MonumentArrived = false;
                RouteStarted = true;
                RouteCompleted = false;
                NextPointIsLast = false;
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
