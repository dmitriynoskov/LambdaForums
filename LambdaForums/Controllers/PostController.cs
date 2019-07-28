using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Models;
using LambdaForums.Models.Post;
using LambdaForums.Models.PostReply;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForums.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPost _postService;
        private readonly IForum _forumService;
        private readonly IApplicationUser _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(
            IPost postService, 
            IForum forumService, 
            UserManager<ApplicationUser> userManager, 
            IApplicationUser userService)
        {
            _postService = postService;
            _forumService = forumService;
            _userManager = userManager;
            _userService = userService;
        }

        [AllowAnonymous]
        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);

            var replies = BuildPostReplies(post.PostReplies);

            var model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImageUrl,
                Created = post.Created,
                AuthorRating = post.User.Rating,
                PostContent = post.Content,
                Replies = replies,
                ForumId = post.ForumId,
                ForumName = post.Forum.Title,
                IsAuthorAdmin = GetIsAuthorAdmin(post.User)
            };
            return View(model);
        }

        public IActionResult Create(int id)
        {
            var forum = _forumService.GetById(id);

            var model = new NewPostModel
            {
                ForumName = forum.Title,
                ForumId = forum.Id,
                ForumImageUrl = forum.ImageUrl,
                AuthorName = User.Identity.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(NewPostModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var post = BuildPost(model, user);
            await _postService.Add(post);
            await _userService.UpdateUserRating(userId, typeof(Post));

            return RedirectToAction("Index", "Post", new {id = post.Id});
        }

        private Post BuildPost(NewPostModel model, ApplicationUser user)
        {
            var forum = _forumService.GetById(model.ForumId);
            return new Post
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user,
                Forum = forum
            };
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(ICollection<PostReply> postReplies)
        {
            return postReplies.Select(r => new PostReplyModel
            {
                Id = r.Id,
                AuthorId = r.User.Id,
                Created = r.Created,
                AuthorRating = r.User.Rating,
                AuthorImageUrl = r.User.ProfileImageUrl,
                AuthorName = r.User.UserName,
                PostId = r.Post.Id,
                ReplyContent = r.Content,
                IsAuthorAdmin = GetIsAuthorAdmin(r.User)
            });
        }
        
        private bool GetIsAuthorAdmin(ApplicationUser postUser)
        {
            return _userManager.GetRolesAsync(postUser)
                .Result
                .Contains("Admin");
        }
    }
}