using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace DataLayer
{
    public interface IForum
    {
        Forum GetById(int id);
        IEnumerable<Forum> GetAll();

        IEnumerable<ApplicationUser> GetAllApplicationUsers();

        Task CreateForum(Forum forum);
        Task DeleteForum(int id);
        Task UpdateForumTitle(int forumId, string forumTitle);
        Task UpdateForumDescription(int forumId, string description);
    }
}
