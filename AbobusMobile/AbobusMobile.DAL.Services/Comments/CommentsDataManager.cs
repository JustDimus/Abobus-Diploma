using AbobusMobile.DAL.Services.Abstractions.Comments;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Comments
{
    public class CommentsDataManager : ICommentsDataManager
    {
        private readonly IRepository<MonumentCommentModel> _monumentComments;
        private readonly IRepository<RouteCommentModel> _routeComments;

        public CommentsDataManager(
            IRepository<MonumentCommentModel> monumentCommentsRepository,
            IRepository<RouteCommentModel> routeCommentsRepository)
        {
            _routeComments = routeCommentsRepository ?? throw new ArgumentNullException(nameof(routeCommentsRepository));
            _monumentComments = monumentCommentsRepository ?? throw new ArgumentNullException(nameof(monumentCommentsRepository));
        }

        public async Task<List<MonumentCommentDataModel>> GetMonumentComments(Guid monumentId)
        {
            var result = new List<MonumentCommentDataModel>();

            var comments = await _monumentComments.SelectAsync(i => i.MonumentId == monumentId);

            foreach (var comment in comments)
            {
                result.Add(new MonumentCommentDataModel()
                {
                    Id = comment.CommentId,
                    CommentText = comment.CommentText,
                    MonumentId = comment.MonumentId,
                    OwnerId = comment.OwnerId,
                });
            }

            return result;
        }

        public async Task<List<RouteCommentDataModel>> GetRouteComments(Guid routeId)
        {
            var result = new List<RouteCommentDataModel>();

            var comments = await _routeComments.SelectAsync(i => i.RouteId == routeId);

            foreach (var comment in comments)
            {
                result.Add(new RouteCommentDataModel()
                {
                    Id = comment.CommentId,
                    CommentText = comment.CommentText,
                    RouteId = comment.RouteId,
                    OwnerId = comment.OwnerId,
                });
            }

            return result;
        }

        public async Task UpdateMonumentComments(List<MonumentCommentDataModel> monumentComments)
        {
            foreach (var comment in monumentComments)
            {
                var existedComment = await _monumentComments.FirstOrDefaultAsync(i => i.CommentId == comment.Id);

                if (existedComment != null)
                {
                    await _monumentComments.DeleteAsync(existedComment);
                }

                await _monumentComments.InsertAsync(new MonumentCommentModel()
                {
                    CommentId = comment.Id,
                    CommentText = comment.CommentText,
                    MonumentId = comment.MonumentId,
                    OwnerId = comment.OwnerId,
                });
            }
        }

        public async Task UpdateRouteComments(List<RouteCommentDataModel> routeComments)
        {
            foreach (var comment in routeComments)
            {
                var existedComment = await _routeComments.FirstOrDefaultAsync(i => i.CommentId == comment.Id);

                if (existedComment != null)
                {
                    await _routeComments.DeleteAsync(existedComment);
                }

                await _routeComments.InsertAsync(new RouteCommentModel()
                {
                    CommentId = comment.Id,
                    CommentText = comment.CommentText,
                    RouteId = comment.RouteId,
                    OwnerId = comment.OwnerId,
                });
            }
        }
    }
}
