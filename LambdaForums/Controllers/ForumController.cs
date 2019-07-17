using System.Linq;
using DataLayer;
using DataLayer.Models;
using LambdaForums.Models.Forum;
using LambdaForums.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForums.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;

        public ForumController(IForum forumService, IPost postService)
        {
            _forumService = forumService;
            _postService = postService;
        }

        public IActionResult Index()
        {
            var forums = _forumService.GetAll()
                .Select(f => new ForumListingModel
                {
                    Id = f.Id,
                    Name = f.Title,
                    Description = f.Description
                });
            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);

            var posts = _postService.GetFilteredPosts(forum, searchQuery);

            var postListings = posts
                .Select(p => new PostListingModel
                {
                    Id = p.Id,
                    Author = p.User.UserName,
                    DatePosted = p.Created,
                    Title = p.Title,
                    AuthorId = p.User.Id,
                    AuthorRating = p.User.Rating,
                    RepliesCount = p.PostReplies.Count,
                    Forum = BuildForumListing(p)
                });

            var model = new ForumTopicModel
            {
                Posts = postListings,
                Forum = BuildForumListing(forum)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Topic", new {id, searchQuery});
        }

        private ForumListingModel BuildForumListing(Forum forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Description = forum.Description,
                Name = forum.Title,
                ForumImageUrl = forum.ImageUrl
            };
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }
    }
}