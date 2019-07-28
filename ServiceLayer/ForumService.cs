using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task CreateForum(Forum forum)
        {
            _context.Forums.Add(forum);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteForum(int id)
        {
            var forum = GetById(id);
            _context.Remove(forum);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateForumTitle(int forumId, string forumTitle)
        {
            var forum = GetById(forumId);
            forum.Title = forumTitle;
            _context.Forums.Update(forum);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateForumDescription(int forumId, string description)
        {
            var forum = GetById(forumId);
            forum.Description = description;
            _context.Forums.Update(forum);
            await _context.SaveChangesAsync();
        }
    }
}
