using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Models;

namespace DataLayer
{
    public interface IForum
    {
        Forum GetById(int id);
        IEnumerable<Forum> GetAll();

        Task CreateForum(Forum forum);
        Task DeleteForum(int id);
        Task UpdateForumTitle(int forumId, string forumTitle);
        Task UpdateForumDescription(int forumId, string description);
        IEnumerable<ApplicationUser> GetActiveUsers(int forumId);
        bool HasRecentPost(int forumId);
    }
}
