using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.DAL.Services.Abstractions.Comments
{
    public class RouteCommentDataModel
    {
        public Guid Id { get; set; }

        public Guid RouteId { get; set; }

        public Guid OwnerId { get; set; }

        public string CommentText { get; set; }
    }
}
