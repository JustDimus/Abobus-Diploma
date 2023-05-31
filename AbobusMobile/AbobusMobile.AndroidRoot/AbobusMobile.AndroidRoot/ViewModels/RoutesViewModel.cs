using AbobusMobile.AndroidRoot.DataExchangeService;
using AbobusMobile.AndroidRoot.Helpers;
using AbobusMobile.AndroidRoot.Models;
using AbobusMobile.AndroidRoot.Views;
using AbobusMobile.BLL.Services.Abstractions.Accounts;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.BLL.Services.Abstractions.Routes;
using AbobusMobile.BLL.Services.Abstractions.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class RoutesViewModel : BaseViewModel
    {
        public class RouteModel
        {
            private ImageSource routeImage;

            public Guid Id { get; set; }

            public string Name { get; set; }
            
            public bool Downloaded { get; set; }

            public int Distance { get; set; }

            public string DistanceUnit { get; set; }

            public MemoryStream RouteImageSource { get; set; }

            public ImageSource RouteImage => routeImage ?? (routeImage = ImageHelper.Create(RouteImageSource));
        }

        private readonly ILocationService _locationService;
        private readonly IRouteService _routeService;
        private readonly IAccountsService _accountService;
        private readonly IResourcesService _resourcesService;
        private readonly RouteExchangeService _routeExchangeService;

        public RoutesViewModel(
            ILocationService locationService,
            IRouteService routeService,
            IAccountsService accountService,
            IResourcesService resourcesService,
            RouteExchangeService routeExchangeService)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _resourcesService = resourcesService ?? throw new ArgumentNullException(nameof(resourcesService));
            _routeExchangeService = routeExchangeService ?? throw new ArgumentNullException(nameof(routeExchangeService));

            ChangeCityCommand = new Command(async () => await OnChangeCityClicked(), () => PageUpdateAvailable());
            UpdatePageCommand = new Command(async () => await UpdatePageAsync(), () => PageUpdateAvailable());
            OpenRouteDetailsCommand = new Command<Guid>(async (routeId) => await OpenRouteDetailsAsync(routeId), (_) => PageUpdateAvailable());

            PropertyChanged += (_, __) =>
            {
                ChangeCityCommand.ChangeCanExecute();
                UpdatePageCommand.ChangeCanExecute();
                OpenRouteDetailsCommand.ChangeCanExecute();
            };
        }

        #region Commands
        public Command UpdatePageCommand { get; }
        public Command ChangeCityCommand { get; }
        public Command<Guid> OpenRouteDetailsCommand { get; }
        #endregion

        #region Properties
        private bool cityFound = false;
        public bool CityFound
        {
            get => cityFound;
            set => SetProperty(ref cityFound, value);
        }

        private LocationServiceModel currentLocation;
        private string currentCity;
        public string CurrentCity
        {
            get => currentCity;
            set => SetProperty(ref currentCity, value);
        }

        private bool updateRequired = true;
        public bool UpdateRequired
        {
            get => updateRequired;
            set => SetProperty(ref updateRequired, value);
        }

        private List<RouteModel> downloadedRoutes;
        public TwoColumnsList<RouteModel> DownloadedRoutes
            => TwoColumnsList<RouteModel>.FromList(downloadedRoutes);

        private bool showDownloadedRoutes = false;
        public bool ShowDownloadedRoutes
        {
            get => showDownloadedRoutes;
            set => SetProperty(ref showDownloadedRoutes, value);
        }

        private List<RouteModel> userRoutes;
        public TwoColumnsList<RouteModel> UserRoutes
            => TwoColumnsList<RouteModel>.FromList(userRoutes);
        private bool showUserRoutes = false;
        public bool ShowUserRoutes
        {
            get => showUserRoutes;
            set => SetProperty(ref showUserRoutes, value);
        }

        private List<RouteModel> availableRoutes;
        public TwoColumnsList<RouteModel> AvailableRoutes
            => TwoColumnsList<RouteModel>.FromList(availableRoutes);
        private bool showAvailableRoutes = false;
        public bool ShowAvailableRoutes
        {
            get => showAvailableRoutes;
            set => SetProperty(ref showAvailableRoutes, value);
        }
        #endregion

        protected override async void OnPageAppeared()
        {
            if (!CityFound)
            {
                await LoadCurrentCityAsync();
            }

            if (UpdateRequired)
            {
                await UpdatePageAsync();
            }
        }

        private async Task OpenRouteDetailsAsync(Guid routeId)
        {
            _routeExchangeService.RequestRoute(routeId);
            await Shell.Current.GoToAsync(PathConstants.ROUTE_DETAILS_ABSOLUTE);
        }

        private async Task LoadCurrentCityAsync()
        {
            CityFound = false;

            currentLocation = await _locationService.GetCurrentLocationAsync();

            if (currentLocation.LocationFound)
            {
                CurrentCity = currentLocation.CityName;
                CityFound = true;
            }
            else
            {
                CurrentCity = null;
            }
        }

        private async Task UpdatePageAsync()
        {
            UpdateRequired = true;

            var routes = await _routeService.GetRoutesDetailsByCityId(currentLocation.CityId);

            var currentAccountId = await _accountService.GetCurrentAccountIdAsync();

            downloadedRoutes = await ConvertRoutesAsync(routes.Where(i => i.Downloaded));
            showDownloadedRoutes = downloadedRoutes.Count() > 0;

            userRoutes = await ConvertRoutesAsync(routes.Where(i => i.CreatorId == currentAccountId));
            showUserRoutes = userRoutes.Count() > 0;

            availableRoutes = await ConvertRoutesAsync(routes.Where(i => i.CreatorId != currentAccountId && !i.Downloaded));
            showAvailableRoutes = availableRoutes.Count() > 0;

            CallRoutesPropertyChanged();

            UpdateRequired = false;
        }
        
        private void CallRoutesPropertyChanged()
        {
            OnPropertyChanged(nameof(AvailableRoutes));
            OnPropertyChanged(nameof(UserRoutes));
            OnPropertyChanged(nameof(DownloadedRoutes));

            OnPropertyChanged(nameof(ShowDownloadedRoutes));
            OnPropertyChanged(nameof(ShowUserRoutes));
            OnPropertyChanged(nameof(ShowAvailableRoutes));
        }

        private async Task OnChangeCityClicked()
        {
            CurrentCity = CurrentCity + "w";
        }

        private async Task<RouteModel> ConvertRouteAsync(RouteDetailsServiceModel route)
        {
            var routeImageResource = await _resourcesService.GetResourceAsync(route.RouteImageId);

            return new RouteModel()
            {
                Id = route.Id,
                Downloaded = route.Downloaded,
                Distance = route.Distance,
                DistanceUnit = route.DistanceUnit,
                Name = route.Name,
                RouteImageSource = routeImageResource.Resource
            };
        }
        private async Task<List<RouteModel>> ConvertRoutesAsync(IEnumerable<RouteDetailsServiceModel> routes)
        {
            List<RouteModel> result = new List<RouteModel>();

            foreach (var route in routes)
            {
                result.Add(await ConvertRouteAsync(route));
            }

            return result;
        }

        private bool PageUpdateAvailable()
            => !UpdateRequired;
    }
}
