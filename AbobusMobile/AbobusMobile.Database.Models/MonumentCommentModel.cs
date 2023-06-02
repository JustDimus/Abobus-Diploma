using AbobusMobile.Database.Services.Abstractions.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Models
{
    [Table("MonumentComments")]
    public class MonumentCommentModel : BaseModel
    {
        [Column("CommentID")]
        public Guid CommentId { get; set; }
        [Column("MonumentID")]
        public Guid MonumentId { get; set; }
        [Column("OwnerID")]
        public Guid OwnerId { get; set; }
        [Column("CommentText")]
        public string CommentText { get; set; }
    }
}
