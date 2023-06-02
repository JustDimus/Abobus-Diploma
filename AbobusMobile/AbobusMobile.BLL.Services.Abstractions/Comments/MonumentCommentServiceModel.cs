using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.BLL.Services.Abstractions.Comments
{
    public class MonumentCommentServiceModel
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public Guid MonumentId { get; set; }

        public string CommentText { get; set; }
    }
}
