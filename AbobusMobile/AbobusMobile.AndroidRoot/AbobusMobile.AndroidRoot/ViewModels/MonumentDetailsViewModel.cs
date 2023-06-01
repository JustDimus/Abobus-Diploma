using AbobusMobile.AndroidRoot.DataExchangeService;
using AbobusMobile.AndroidRoot.Helpers;
using AbobusMobile.BLL.Services.Abstractions.Accounts;
using AbobusMobile.BLL.Services.Abstractions.Comments;
using AbobusMobile.BLL.Services.Abstractions.Monuments;
using AbobusMobile.BLL.Services.Abstractions.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class MonumentDetailsViewModel : BaseViewModel
    {
        public class CommentModel
        {
            public string Username { get; set; }

            public string CommentText { get; set; }
        }

        private readonly ExchangeService _exchangeService;
        private readonly IMonumentsService _monumentsService;
        private readonly IResourcesService _resourcesService;
        private readonly ICommentsService _commentsService;
        private readonly IAccountsService _accountsService;

        private IDisposable exchangeServiceDisposable;

        public MonumentDetailsViewModel(
            ExchangeService exchangeService,
            IMonumentsService monumentsService,
            IResourcesService resourcesService,
            ICommentsService commentsService,
            IAccountsService accountsService)
        {
            _exchangeService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));
            _monumentsService = monumentsService ?? throw new ArgumentNullException(nameof(monumentsService));
            _resourcesService = resourcesService ?? throw new ArgumentNullException(nameof(resourcesService));
            _commentsService = commentsService ?? throw new ArgumentNullException(nameof(commentsService));
            _accountsService = accountsService ?? throw new ArgumentNullException(nameof(accountsService));

            exchangeServiceDisposable = _exchangeService.OnMonumentRequested
                .Subscribe(OnMonumentRequested);

            ImageSwitchCommand = new Command((value) => SwitchImage(Convert.ToInt32(value)), (_) => ImageSwitchAvailable());
            CommentSwitchCommand = new Command((value) => SwitchComment(Convert.ToInt32(value)), (_) => CommentSwitchAvailable());

            PropertyChanged += (_, __) =>
            {
                ImageSwitchCommand.ChangeCanExecute();
                CommentSwitchCommand.ChangeCanExecute();
            };
        }

        #region Commands
        public Command ImageSwitchCommand { get; }
        public Command CommentSwitchCommand { get; }
        #endregion

        private List<ImageSource> monumentImages = new List<ImageSource>();
        private List<CommentModel> monumentComments = new List<CommentModel>();

        private Guid? RequestedRouteId { get; set; }

        #region Properties
        private bool updateReqired = true;
        public bool UpdateRequired
        {
            get => updateReqired;
            set => SetProperty(ref updateReqired, value);
        }

        private string monumentName;
        public string MonumentName
        {
            get => monumentName;
            set => SetProperty(ref monumentName, value);
        }

        private ImageSource currentImage;
        public ImageSource CurrentImage
        {
            get => currentImage;
            set => SetProperty(ref currentImage, value);
        }

        private string monumentDescription;
        public string MonumentDescription
        {
            get => monumentDescription;
            set => SetProperty(ref monumentDescription, value);
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

        public bool CommentSwitchAvailable()
            => BaseActionAvailability() && monumentComments.Count > 1;

        public bool ImageSwitchAvailable()
            => BaseActionAvailability() && monumentImages.Count > 1;

        public bool BaseActionAvailability()
            => !UpdateRequired;

        private async Task UpdatePageAsync()
        {
            UpdateRequired = true;

            if (!RequestedRouteId.HasValue)
            {
                // Handle error here
                return;
            }

            var monumentDetails = await _monumentsService.GetMonumentAsync(RequestedRouteId.Value);

            var monumentImages = await _monumentsService.GetMonumentImagesAsync(monumentDetails.Id);

            var monumentComments = await _commentsService.GetMonumentCommentsAsync(monumentDetails.Id);

            MonumentName = monumentDetails.Name;
            MonumentDescription = monumentDetails.Description;

            await UpdateMonumentImages(monumentImages);
            await UpdateMonumentComments(monumentComments);

            UpdateRequired = false;
        }

        private async Task UpdateMonumentImages(List<MonumentImageServiceModel> monumentImageModels)
        {
            monumentImages.Clear();

            foreach (var image in monumentImageModels)
            {
                var imageResource = await _resourcesService.GetResourceAsync(image.ImageId);

                monumentImages.Add(ImageHelper.Create(imageResource.Resource));
            }

            CurrentImage = monumentImages.FirstOrDefault();
        }

        private async Task UpdateMonumentComments(List<MonumentCommentServiceModel> monumentCommentModels)
        {
            monumentComments.Clear();

            foreach (var comment in monumentCommentModels)
            {
                var accountCommentOwner = await _accountsService.LoadAccountInfo(comment.OwnerId);

                monumentComments.Add(new CommentModel()
                {
                    Username = accountCommentOwner.Username,
                    CommentText = comment.CommentText,
                });
            }

            CurrentComment = monumentComments.FirstOrDefault();
        }

        private void SwitchImage(int direction)
        {
            var currentIndex = monumentImages.IndexOf(CurrentImage);

            currentIndex += direction;

            if (currentIndex < 0)
            {
                currentIndex = monumentImages.Count - 1;
            }
            else if (currentIndex >= monumentImages.Count)
            {
                currentIndex = 0;
            }

            CurrentImage = monumentImages[currentIndex];
        }

        private void SwitchComment(int direction)
        {
            var currentIndex = monumentComments.IndexOf(CurrentComment);

            currentIndex += direction;

            if (currentIndex < 0)
            {
                currentIndex = monumentComments.Count - 1;
            }
            else if (currentIndex >= monumentComments.Count)
            {
                currentIndex = 0;
            }

            CurrentComment = monumentComments[currentIndex];
        }

        private void OnMonumentRequested(Guid monumentId)
        {
            RequestedRouteId = monumentId;
            UpdateRequired = true;
        }
    }
}
