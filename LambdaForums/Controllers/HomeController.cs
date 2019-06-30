using System.Diagnostics;
using System.Linq;
using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using LambdaForums.Models;
using LambdaForums.Models.Forum;
using LambdaForums.Models.Home;
using LambdaForums.Models.Post;

namespace LambdaForums.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPost _postService;
        
        public HomeController(
            IPost postService, 
            IForum forumService)
        {
            _postService = postService;
        }

        public IActionResult Index()
        {
            var model = BuildHomeIndexModel();
            return View(model);
        }
        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private HomeIndexModel BuildHomeIndexModel()
        {
            var latestPosts = _postService.GetLatestPosts(10);

            var posts = latestPosts
                .Select(p => new PostListingModel
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

            return new HomeIndexModel
            {
                LatestPosts = posts,
                SearchQuery = ""
            };
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
