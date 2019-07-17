using System.Collections.Generic;
using LambdaForums.Models.Post;

namespace LambdaForums.Models.Search
{
    public class SearchResultModel
    {
        public IEnumerable<PostListingModel> Posts { get; set; }

        public string SearchQuery { get; set; }

        public bool EmptySearchResults { get; set; }
    }
}
