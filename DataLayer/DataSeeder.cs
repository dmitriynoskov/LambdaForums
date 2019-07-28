using System;
using System.Linq;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataLayer
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SeedSuperUser()
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var user = new ApplicationUser
            {
                UserName = "superadmin@ww.ww",
                NormalizedUserName = "SUPERADMIN@WW.WW",
                Email = "superadmin@ww.ww",
                NormalizedEmail = "SUPERADMIN@WW.WW",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var hashedPassword = hasher.HashPassword(user, "Test12!");

            user.PasswordHash = hashedPassword;

            var roleStore = new RoleStore<IdentityRole>(_context);
            var isAdminRoleExists = _context.Roles.Any(r => r.Name == "Admin");
            if (!isAdminRoleExists)
            {
                roleStore.CreateAsync(new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin"
                }).Wait();
            }

            var userStore = new UserStore<ApplicationUser>(_context);
            var isSuperAdminExists = _context.Users.Any(u => u.UserName == user.UserName);

            if (!isSuperAdminExists)
            {
                userStore.CreateAsync(user).Wait();
                userStore.AddToRoleAsync(user, "Admin").Wait();
            }

            _context.SaveChangesAsync().Wait();
        }
    }
}
