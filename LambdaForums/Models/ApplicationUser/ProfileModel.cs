using System;
using Microsoft.AspNetCore.Http;

namespace LambdaForums.Models.ApplicationUser
{
    public class ProfileModel
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public int UserRating { get; set; }

        public string ProfileImageUrl { get; set; }

        public DateTime MemberSince { get; set; }

        public bool IsAdmin { get; set; }

        public IFormFile ImageUpload { get; set; }
    }
}
