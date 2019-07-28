using System.Net.Http.Headers;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Models;
using LambdaForums.Models.ApplicationUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LambdaForums.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        public ProfileController(
            UserManager<ApplicationUser> userManager, 
            IApplicationUser userService, 
            IUpload uploadService, IConfiguration configuration)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
            _configuration = configuration;
        }

        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;

            var model = new ProfileModel
            {
                UserName = user.UserName,
                Email = user.Email,
                MemberSince = user.MemberSince,
                ProfileImageUrl = user.ProfileImageUrl,
                UserId = user.Id,
                UserRating = user.Rating.ToString(),
                IsAdmin = userRoles.Contains("Admin")
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);

            //Connect to an Azure storage account container
            var connectionString = _configuration.GetConnectionString("AzureStorage");

            //Get Blob container
            var container = _uploadService.GetBlobContainer(connectionString, "profile-images");

            //Parse the Content Disposition response header
            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            //Grab the filename
            var fileName = contentDisposition.FileName.Trim('"');

            //Get a reference to Block Blob
            var blockBlob = container.GetBlockBlobReference(fileName);

            //On that block blob, upload our file <-- file uploaded to the cloud
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());

            //Set the User's profile to the Uri
            await _userService.SetProfileImage(userId, blockBlob.Uri);

            //Redirect to the User's profile page
            return RedirectToAction("Detail", "Profile", new {id = userId});
        }
    }
}