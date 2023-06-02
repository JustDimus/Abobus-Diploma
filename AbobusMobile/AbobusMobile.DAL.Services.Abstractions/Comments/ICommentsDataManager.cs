using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.DAL.Services.Abstractions.Comments
{
    public interface ICommentsDataManager
    {
        Task<List<RouteCommentDataModel>> GetRouteComments(Guid routeId);
        Task UpdateRouteComments(List<RouteCommentDataModel> routeComments);

        Task<List<MonumentCommentDataModel>> GetMonumentComments(Guid monumentId);
        Task UpdateMonumentComments(List<MonumentCommentDataModel> monumentComments);
    }
}
