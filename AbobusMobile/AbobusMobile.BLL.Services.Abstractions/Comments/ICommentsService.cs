using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Abstractions.Comments
{
    public interface ICommentsService
    {
        Task<List<MonumentCommentServiceModel>> GetMonumentCommentsAsync(Guid monumentId);

        Task<List<MonumentCommentServiceModel>> GetRouteCommentsAsync(Guid routeId);
    }
}
