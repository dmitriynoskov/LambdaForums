using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Models;
using LambdaForums.Models.Post;
using LambdaForums.Models.PostReply;
using Microsoft.AspNetCore.Mvc;

namespace LambdaForums.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost _postService;

        public PostController(IPost postService)
        {
            _postService = postService;
        }

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
                Replies = replies
            };
            return View(model);
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
                ReplyContent = r.Content
            });
        }
    }
}