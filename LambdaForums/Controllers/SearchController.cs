using System.Linq;
using DataLayer;
using DataLayer.Models;
using LambdaForums.Models.Forum;
using LambdaForums.Models.Post;
using LambdaForums.Models.Search;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForums.Controllers
{
    public class SearchController : Controller
    {
        private readonly IPost _postService;

        public SearchController(IPost postService)
        {
            _postService = postService;
        }

        public IActionResult Results(string searchQuery)
        {
            var posts = _postService.GetFilteredPosts(searchQuery);

            var areNoResults = !string.IsNullOrEmpty(searchQuery) && !posts.Any();

            var postListings = posts.Select(p => new PostListingModel
            {
                Title = p.Title,
                Author = p.User.UserName,
                AuthorRating = p.User.Rating,
                AuthorId = p.User.Id,
                DatePosted = p.Created,
                Id = p.Id,
                RepliesCount = p.PostReplies.Count,
                Forum = BuildForumListingForPost(p)
            });

            var model = new SearchResultModel
            {
                Posts = postListings,
                SearchQuery = searchQuery,
                EmptySearchResults = areNoResults
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return RedirectToAction("Results", new { searchQuery });
        }

        private ForumListingModel BuildForumListingForPost(Post post)
        {
            var forum = post.Forum;

            return new ForumListingModel
            {
                Id = forum.Id,
                Description = forum.Description,
                ForumImageUrl = forum.ImageUrl,
                Name = forum.Title
            };
        }
    }

}
