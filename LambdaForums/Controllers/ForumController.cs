using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Models;
using LambdaForums.Models.Forum;
using LambdaForums.Models.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LambdaForums.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        public ForumController(
            IForum forumService, 
            IPost postService, 
            IUpload uploadService, 
            IConfiguration configuration)
        {
            _forumService = forumService;
            _postService = postService;
            _uploadService = uploadService;
            _configuration = configuration;
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

        public IActionResult Create()
        {
            var model = new AddForumModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddForum(AddForumModel model)
        {
            var imageUri = "/images/users/default.png";
            if (model.ImageUpload != null)
            {
                var blockBlob = await UploadForumImage(model.ImageUpload);

                imageUri = blockBlob.Uri.AbsoluteUri;
            }

            var forum = new Forum
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageUrl = imageUri
            };

            await _forumService.CreateForum(forum);

            return RedirectToAction("Index", "Forum");
        }

        private async Task<CloudBlockBlob> UploadForumImage(IFormFile file)
        {
            var connectionString = _configuration.GetConnectionString("AzureStorage");
            var container = _uploadService.GetBlobContainer(connectionString, "forum-images");
            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var fileName = contentDisposition.FileName.Trim('"');
            var blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            return blockBlob;
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