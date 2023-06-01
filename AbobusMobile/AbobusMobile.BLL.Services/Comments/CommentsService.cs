using AbobusCore.Models.Comments;
using AbobusMobile.BLL.Services.Abstractions.Comments;
using AbobusMobile.Communication.Requests.Comments;
using AbobusMobile.Communication.Services.Abstractions;
using AbobusMobile.DAL.Services.Abstractions.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly IRequestFactory _requestFactory;
        private readonly ICommentsDataManager _commentsManager;

        private GetRouteCommentsRequest routeCommentsRequest;
        private GetMonumentCommentsRequest monumentCommentsRequest;

        public CommentsService(
            IRequestFactory requestFactory,
            ICommentsDataManager commentsDataManager)
        {
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
            _commentsManager = commentsDataManager ?? throw new ArgumentNullException(nameof(commentsDataManager));
        }

        private GetRouteCommentsRequest RouteCommentsRequest
            => routeCommentsRequest ?? (routeCommentsRequest = _requestFactory.CreateRequest<GetRouteCommentsRequest>());
        private GetMonumentCommentsRequest MonumentCommentsRequest
            => monumentCommentsRequest ?? (monumentCommentsRequest = _requestFactory.CreateRequest<GetMonumentCommentsRequest>());

        public async Task<List<MonumentCommentServiceModel>> GetMonumentCommentsAsync(Guid monumentId)
        {
            var loadedComments = await _commentsManager.GetMonumentComments(monumentId);

            MonumentCommentsRequest.Initialize(monumentId);

            var commentsResponse = await MonumentCommentsRequest.SendRequestAsync();

            var result = new List<MonumentCommentServiceModel>();

            result.AddRange(GetCommentServiceModel(loadedComments));

            if (commentsResponse.Succeeded)
            {
                var newComments = commentsResponse.As<List<MonumentCommentModel>>();

                await _commentsManager.UpdateMonumentComments(GetCommentDataModel(newComments));

                result.Clear();
                result.AddRange(GetCommentServiceModel(newComments));
            }

            return result;
        }

        public async Task<List<RouteCommentServiceModel>> GetRouteCommentsAsync(Guid routeId)
        {
            var loadedComments = await _commentsManager.GetRouteComments(routeId);

            MonumentCommentsRequest.Initialize(routeId);

            var commentsResponse = await MonumentCommentsRequest.SendRequestAsync();

            var result = new List<RouteCommentServiceModel>();

            result.AddRange(GetCommentServiceModel(loadedComments));

            if (commentsResponse.Succeeded)
            {
                var newComments = commentsResponse.As<List<RouteCommentModel>>();

                await _commentsManager.UpdateRouteComments(GetCommentDataModel(newComments));

                result.Clear();
                result.AddRange(GetCommentServiceModel(newComments));
            }

            return result;
        }

        private List<RouteCommentDataModel> GetCommentDataModel(List<RouteCommentModel> comments)
            => comments.Select(i => new RouteCommentDataModel()
            {
                CommentText = i.CommentText,
                RouteId = i.RouteId,
                OwnerId = i.OwnerId,
            }).ToList();

        private List<RouteCommentServiceModel> GetCommentServiceModel(List<RouteCommentModel> comments)
            => comments.Select(i => new RouteCommentServiceModel()
            {
                OwnerId = i.OwnerId,
                CommentText = i.CommentText,
                RouteId = i.RouteId,
            }).ToList();

        private List<RouteCommentServiceModel> GetCommentServiceModel(List<RouteCommentDataModel> comments)
            => comments.Select(i => new RouteCommentServiceModel()
            {
                OwnerId = i.OwnerId,
                CommentText = i.CommentText,
                RouteId = i.RouteId,
            }).ToList();

        private List<MonumentCommentDataModel> GetCommentDataModel(List<MonumentCommentModel> comments)
            => comments.Select(i => new MonumentCommentDataModel()
            {
                CommentText = i.CommentText,
                MonumentId = i.MonumentId,
                OwnerId = i.OwnerId,
            }).ToList();

        private List<MonumentCommentServiceModel> GetCommentServiceModel(List<MonumentCommentModel> comments)
            => comments.Select(i => new MonumentCommentServiceModel()
            {
                OwnerId = i.OwnerId,
                CommentText = i.CommentText,
                MonumentId = i.MonumentId,
            }).ToList();

        private List<MonumentCommentServiceModel> GetCommentServiceModel(List<MonumentCommentDataModel> comments)
            => comments.Select(i => new MonumentCommentServiceModel()
            {
                OwnerId = i.OwnerId,
                CommentText = i.CommentText,
                MonumentId = i.MonumentId,
            }).ToList();
    }
}
