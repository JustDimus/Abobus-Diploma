using AbobusMobile.Database.Services.Abstractions.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbobusMobile.Database.Models
{
    [Table("RouteComments")]
    public class RouteCommentModel : BaseModel
    {
        [Column("CommentID")]
        public Guid CommentId { get; set; }
        [Column("RouteID")]
        public Guid RouteId { get; set; }
        [Column("OwnerID")]
        public Guid OwnerId { get; set; }
        [Column("CommentText")]
        public string CommentText { get; set; }
    }
}
