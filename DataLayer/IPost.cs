﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Models;

namespace DataLayer
{
    public interface IPost
    {
        Post GetById(int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetFilteredPosts(string searchQuery);
        IEnumerable<Post> GetPostsByForum(int forumId);

        Task Add(Post post);
        Task Delete(int id);
        Task EditPost(int id, string content);

        Task AddReply(PostReply postReply);
    }
}