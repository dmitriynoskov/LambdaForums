using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Models;

namespace ServiceLayer
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser GetById(string id)
        {
            return _context.Users.Find(id);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.Users;
        }

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;

            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserRating(string id, Type type)
        {
            var user = GetById(id);
            user.Rating = CalculateUserRating(type, user.Rating);

            await _context.SaveChangesAsync();
        }

        private int CalculateUserRating(Type type, int rating)
        {
            var inc = 0;
            if (type == typeof(Post))
            {
                inc = 1;
            }

            if (type == typeof(PostReply))
            {
                inc = 3;
            }

            return rating + inc;
        }
    }
}
