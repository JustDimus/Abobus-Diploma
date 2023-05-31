using AbobusMobile.AndroidRoot.DataExchangeService;
using AbobusMobile.AndroidRoot.Helpers;
using AbobusMobile.AndroidRoot.Views;
using AbobusMobile.BLL.Services.Abstractions.Comments;
using AbobusMobile.BLL.Services.Abstractions.Monuments;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.BLL.Services.Abstractions.Routes;
using AbobusMobile.BLL.Services.Abstractions.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class RouteDetailsViewModel : BaseViewModel
    {
        public class MonumentModel
        {
            public Guid Id { get; set; }

            public ImageSource MonumentImage { get; set; }
        }

        public class CommentModel
        {
            public string Username { get; set; }

            public string CommentText { get; set; }
        }

        private readonly IRouteService _routeService;
        private readonly ILocationService _locationService;
        private readonly IResourcesService _resourcesService;
        private readonly IMonumentsService _monumentsService;
        private readonly ICommentsService _commentsService;

        private readonly RouteExchangeService _routeExchangeService;
        private IDisposable routeExchangeServiceDisposable;

        public RouteDetailsViewModel(
            RouteExchangeService routeExchangeService,
            IRouteService routeService,
            ILocationService locationService,
            IResourcesService resourcesService,
            IMonumentsService monumentsService,
            ICommentsService commentsService)
        {
            _routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _resourcesService = resourcesService ?? throw new ArgumentNullException(nameof(resourcesService));
            _monumentsService = monumentsService ?? throw new ArgumentNullException(nameof(monumentsService));

            _routeExchangeService = routeExchangeService ?? throw new ArgumentNullException(nameof(routeExchangeService));

            routeExchangeServiceDisposable = _routeExchangeService.OnRouteRequested
                .Subscribe(OnRouteRequested);
        }

        private Guid? RequestedRouteId { get; set; }

        private List<MonumentModel> routeMonuments;
        private List<CommentModel> commentMonuments;

        #region Properties
        private bool updateRequired = true;
        public bool UpdateRequired
        {
            get => updateRequired;
            set => SetProperty(ref updateRequired, value);
        }

        private string routeName;
        public string RouteName
        {
            get => routeName;
            set => SetProperty(ref routeName, value);
        }

        private ImageSource routeImage;
        public ImageSource RoutePhoto
        {
            get => routeImage;
            set => SetProperty(ref routeImage, value);
        }

        private string cityName;
        public string CityName
        {
            get => cityName;
            set => SetProperty(ref cityName, value);
        }

        private int routeDistance;
        public int RouteDistance
        {
            get => routeDistance;
            set => SetProperty(ref routeDistance, value);
        }

        private string distanceUnit;
        public string DistanceUnit
        {
            get => distanceUnit;
            set => SetProperty(ref distanceUnit, value);
        }

        private MonumentModel currentMonument;
        public MonumentModel CurrentMonument
        {
            get => currentMonument;
            set => SetProperty(ref currentMonument, value);
        }

        private CommentModel currentComment;
        public CommentModel CurrentComment
        {
            get => currentComment;
            set => SetProperty(ref currentComment, value);
        }

        #endregion

        protected override async void OnPageAppeared()
        {
            if (UpdateRequired)
            {
                await UpdatePageAsync();
            }
        }

        private async Task UpdatePageAsync()
        {
            UpdateRequired = true;

            if (!RequestedRouteId.HasValue)
            {
                // Handle error here
                return;
            }

            var routeDetails = await _routeService.GetRouteDetailsAsync(RequestedRouteId.Value);

            var routeLocation = await _locationService.GetLocationAsync(routeDetails.CityId);

            var routeImageResource = await _resourcesService.GetResourceAsync(routeDetails.RouteImageId);
            var routeImageSource = ImageHelper.Create(routeImageResource.Resource);

            var routeMonuments = await _monumentsService.GetMonumentsByRouteIdAsync(routeDetails.Id);
            
            var routeComments = await _commentsService.GetRouteCommentsAsync(routeDetails.Id);
            


            UpdateRequired = false;
        }

        private void OnRouteRequested(Guid routeId)
        {
            RequestedRouteId = routeId;
            UpdateRequired = true;
        }
    }
}
