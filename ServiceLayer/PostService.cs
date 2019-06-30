using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Post GetById(int id)
        {
            return _context.Posts
                .Where(p => p.Id == id)
                .Include(p => p.User)
                .Include(p => p.PostReplies)
                    .ThenInclude(r => r.User)
                .Include(p => p.Forum)
                .FirstOrDefault();
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts
                .Include(p => p.PostReplies);
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetPostsByForum(int forumId)
        {
            return _context.Forums
                .FirstOrDefault(f => f.Id == forumId)?
                .Posts;
        }

        public async Task Add(Post post)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditPost(int id, string content)
        {
            throw new NotImplementedException();
        }

        public Task AddReply(PostReply postReply)
        {
            throw new NotImplementedException();
        }
    }
}
