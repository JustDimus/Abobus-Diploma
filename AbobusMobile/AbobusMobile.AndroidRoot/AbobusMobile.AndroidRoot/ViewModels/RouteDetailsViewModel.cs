using AbobusMobile.AndroidRoot.DataExchangeService;
using AbobusMobile.AndroidRoot.Helpers;
using AbobusMobile.AndroidRoot.Views;
using AbobusMobile.BLL.Services.Abstractions.Accounts;
using AbobusMobile.BLL.Services.Abstractions.Comments;
using AbobusMobile.BLL.Services.Abstractions.Monuments;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using AbobusMobile.BLL.Services.Abstractions.Routes;
using AbobusMobile.BLL.Services.Abstractions.Utilities;
using AbobusMobile.BLL.Services.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
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
        private readonly IAccountsService _accountService;

        private readonly ExchangeService _exchangeService;
        private IDisposable routeExchangeServiceDisposable;

        public RouteDetailsViewModel(
            ExchangeService exchangeService,
            IRouteService routeService,
            ILocationService locationService,
            IResourcesService resourcesService,
            IMonumentsService monumentsService,
            ICommentsService commentsService,
            IAccountsService accountsService)
        {
            _routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _resourcesService = resourcesService ?? throw new ArgumentNullException(nameof(resourcesService));
            _monumentsService = monumentsService ?? throw new ArgumentNullException(nameof(monumentsService));
            _commentsService = commentsService ?? throw new ArgumentNullException(nameof(commentsService));
            _accountService = accountsService ?? throw new ArgumentNullException(nameof(accountsService));

            _exchangeService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));

            routeExchangeServiceDisposable = _exchangeService.OnRouteRequested
                .Subscribe(OnRouteRequested);

            MonumentSwitchCommand = new Command(direction => SwitchMonument(Convert.ToInt32(direction)), (_) => MonumentSwitchAvailable());
            CommentSwitchCommand = new Command(direction => SwitchComment(Convert.ToInt32(direction)), (_) => CommentSwitchAvailable());
            OpenCurrentMonumentDetailsCommand = new Command(async () => await OpenCurrentMonumentDetailsAsync(), () => OpenCurrentMonumentAvailable());
            ChangeResourceStatusCommand = new Command(async () => await ChangeResourceStatusAsync(), () => BaseActionAvailability());
            StartRouteCommand = new Command(async () => await StartRouteAsync(), () => StartRouteAvailable());

            PropertyChanged += (_, __) =>
            {
                MonumentSwitchCommand.ChangeCanExecute();
                OpenCurrentMonumentDetailsCommand.ChangeCanExecute();
                CommentSwitchCommand.ChangeCanExecute();
                ChangeResourceStatusCommand.ChangeCanExecute();
                StartRouteCommand.ChangeCanExecute();
            };
        }

        #region Commands
        public Command StartRouteCommand { get; }
        public Command MonumentSwitchCommand { get; }
        public Command CommentSwitchCommand { get; }
        public Command OpenCurrentMonumentDetailsCommand { get; }
        public Command ChangeResourceStatusCommand { get; }
        #endregion

        private Guid? RequestedRouteId { get; set; }

        private List<MonumentModel> routeMonuments = new List<MonumentModel>();
        private List<CommentModel> routeComments = new List<CommentModel>();

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
        public ImageSource RouteImage
        {
            get => routeImage;
            set => SetProperty(ref routeImage, value);
        }

        private string routeCity;
        public string RouteCity
        {
            get => routeCity;
            set => SetProperty(ref routeCity, value);
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

        private bool downloadingInProgress = false;
        public bool DownloadingInProgress
        {
            get => downloadingInProgress;
            set => SetProperty(ref downloadingInProgress, value);
        }

        private bool routeDownloaded;
        public bool Downloaded
        {
            get => routeDownloaded;
            set => SetProperty(ref routeDownloaded, value);
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

        public bool StartRouteAvailable()
            => BaseActionAvailability() && Downloaded;

        public bool OpenCurrentMonumentAvailable()
            => BaseActionAvailability() && CurrentMonument != null;

        public bool CommentSwitchAvailable()
            => BaseActionAvailability() && routeComments.Count > 1;

        public bool MonumentSwitchAvailable()
            => BaseActionAvailability() && routeMonuments.Count > 1;

        public bool BaseActionAvailability()
            => !UpdateRequired && !DownloadingInProgress;

        private async Task StartRouteAsync()
        {

        }

        private async Task OpenCurrentMonumentDetailsAsync()
        {
            _exchangeService.RequestMonument(CurrentMonument.Id);
            await Shell.Current.GoToAsync(PathConstants.MONUMENT_DETAILS_ABSOLUTE);
        }

        private void SwitchMonument(int direction)
        {
            var currentIndex = routeMonuments.IndexOf(CurrentMonument);

            currentIndex += direction;

            if (currentIndex < 0)
            {
                currentIndex = routeMonuments.Count - 1;
            }
            else if (currentIndex >= routeMonuments.Count)
            {
                currentIndex = 0;
            }

            CurrentMonument = routeMonuments[currentIndex];
        }

        private void SwitchComment(int direction)
        {
            var currentIndex = routeComments.IndexOf(CurrentComment);

            currentIndex += direction;

            if (currentIndex < 0)
            {
                currentIndex = routeComments.Count - 1;
            }
            else if (currentIndex >= routeComments.Count)
            {
                currentIndex = 0;
            }

            CurrentComment = routeComments[currentIndex];
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

            RouteName = routeDetails.Name;
            RouteCity = routeLocation.CityName;
            RouteDistance = routeDetails.Distance;
            DistanceUnit = routeDetails.DistanceUnit;
            Downloaded = routeDetails.Downloaded;
            RouteImage = routeImageSource;

            await UpdateRouteMonuments(routeMonuments);
            await UpdateRouteComments(routeComments);

            UpdateRequired = false;
        }

        private async Task ChangeResourceStatusAsync()
        {
            DownloadingInProgress = true;

            if (Downloaded)
            {
                var resourceStatus = await _routeService.DeleteRouteAsync(RequestedRouteId.Value);

                if (resourceStatus == ResourceServiceStatus.Deleted)
                {
                    Downloaded = false;
                }
            }
            else
            {
                var resourceStatus = await _routeService.DownloadRouteAsync(RequestedRouteId.Value);

                if (resourceStatus == ResourceServiceStatus.Downloaded)
                {
                    Downloaded = true;
                }
            }

            DownloadingInProgress = false;
        }

        private async Task UpdateRouteComments(List<RouteCommentServiceModel> routeCommentsServiceModels)
        {
            routeComments.Clear();

            foreach (var comment in routeCommentsServiceModels)
            {
                routeComments.Add(await ConvertCommentModel(comment));
            }

            CurrentComment = routeComments.FirstOrDefault();
        }

        private async Task<CommentModel> ConvertCommentModel(RouteCommentServiceModel serviceModel)
        {
            var accountCommentOwner = await _accountService.LoadAccountInfo(serviceModel.OwnerId);

            return new CommentModel()
            {
                Username = accountCommentOwner.Username,
                CommentText = serviceModel.CommentText,
            };
        }

        private async Task UpdateRouteMonuments(List<MonumentServiceModel> monumentServiceModels)
        {
            routeMonuments.Clear();

            foreach (var monument in monumentServiceModels)
            {
                routeMonuments.Add(await ConvertMonumentModel(monument));
            }

            CurrentMonument = routeMonuments.FirstOrDefault();
        }

        private async Task<MonumentModel> ConvertMonumentModel(MonumentServiceModel serviceModel)
        {
            var monumentTitleImageResource = await _resourcesService.GetResourceAsync(serviceModel.MonumentTitleImageId);

            return new MonumentModel()
            {
                Id = serviceModel.Id,
                MonumentImage = ImageHelper.Create(monumentTitleImageResource.Resource),
            };
        }

        private void OnRouteRequested(Guid routeId)
        {
            RequestedRouteId = routeId;
            UpdateRequired = true;
        }
    }
}
