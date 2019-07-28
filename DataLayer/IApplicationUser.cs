using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Models;

namespace DataLayer
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();

        Task SetProfileImage(string id, Uri uri);
        Task UpdateUserRating(string id, Type type);
    }
}
