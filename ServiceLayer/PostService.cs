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
                .Include(p => p.User)
                .Include(p => p.PostReplies)
                    .ThenInclude(r => r.User)
                .Include(p => p.Forum);
        }

        public IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery)
        {
            return !string.IsNullOrEmpty(searchQuery)
                ? forum.Posts
                    .Where(p => p.Title.Contains(searchQuery.ToLower()) || p.Content.Contains(searchQuery.ToLower()))
                : forum.Posts;
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            return GetAll()
                .Where(p => p.Title.Contains(searchQuery.ToLower()) || p.Content.Contains(searchQuery.ToLower()));
        }

        public IEnumerable<Post> GetPostsByForum(int forumId)
        {
            return _context.Forums
                .FirstOrDefault(f => f.Id == forumId)?
                .Posts;
        }

        public IEnumerable<Post> GetLatestPosts(int number)
        {
            return GetAll()
                .OrderByDescending(p => p.Created)
                .Take(number);
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
