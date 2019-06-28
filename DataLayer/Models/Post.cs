using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public int ForumId { get; set; }

        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Forum Forum { get; set; }

        public virtual ICollection<PostReply> PostReplies { get; set; } = new HashSet<PostReply>();
    }
}
