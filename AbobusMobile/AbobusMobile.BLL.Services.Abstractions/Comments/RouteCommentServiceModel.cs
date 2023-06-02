using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.BLL.Services.Abstractions.Comments
{
    public class RouteCommentServiceModel
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public Guid RouteId { get; set; }

        public string CommentText { get; set; }
    }
}
