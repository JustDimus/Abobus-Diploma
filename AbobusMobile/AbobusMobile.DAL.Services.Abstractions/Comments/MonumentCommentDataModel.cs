using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.DAL.Services.Abstractions.Comments
{
    public class MonumentCommentDataModel
    {
        public Guid Id { get; set; }

        public Guid MonumentId { get; set; }

        public Guid OwnerId { get; set; }

        public string CommentText { get; set; }
    }
}
