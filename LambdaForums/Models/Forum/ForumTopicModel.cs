using System.Collections.Generic;
using LambdaForums.Models.Post;

namespace LambdaForums.Models.Forum
{
    public class ForumTopicModel
    {
        public ForumListingModel Forum { get; set; }

        public IEnumerable<PostListingModel> Posts { get; set; }
    }
}
