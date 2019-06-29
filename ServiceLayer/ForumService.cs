using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer
{
    public class ForumService : IForum
    {
        private readonly ApplicationDbContext _context;

        public ForumService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Forum GetById(int id)
        {
            var forum = _context.Forums
                .Where(f => f.Id == id)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.User)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.PostReplies)
                        .ThenInclude(pr => pr.User)
                .FirstOrDefault();

            return forum;
        }

        public IEnumerable<Forum> GetAll()
        {
            return _context.Forums
                .Include(f => f.Posts);
        }

        public IEnumerable<ApplicationUser> GetAllApplicationUsers()
        {
            return _context.ApplicationUsers;
        }

        public Task CreateForum(Forum forum)
        {
            throw new NotImplementedException();
        }

        public Task DeleteForum(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumTitle(int forumId, string forumTitle)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumDescription(int forumId, string description)
        {
            throw new NotImplementedException();
        }
    }
}
