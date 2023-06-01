using AbobusMobile.BLL.Services.Abstractions.Comments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Comments
{
    public class CommentsService : ICommentsService
    {
        public Task<List<MonumentCommentServiceModel>> GetMonumentCommentsAsync(Guid monumentId)
        {
            throw new NotImplementedException();
        }

        public Task<List<RouteCommentServiceModel>> GetRouteCommentsAsync(Guid routeId)
        {
            throw new NotImplementedException();
        }
    }
}
