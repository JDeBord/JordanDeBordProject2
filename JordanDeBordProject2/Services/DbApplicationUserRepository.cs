using JordanDeBordProject2.Models.Entities;
using JordanDeBordProject2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public class DbApplicationUserRepository : IApplicationUserRepository
    {
        private ApplicationDbContext _database;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public DbApplicationUserRepository
            (
                ApplicationDbContext database,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager
            )
        {
            _database = database;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApplicationUser> ReadAsync(string userName)
        {
            var user = await _database.Users.FirstOrDefaultAsync(user => user.UserName == userName);
            user.Roles = await _userManager.GetRolesAsync(user);
            return user;
        }

        public async Task<ApplicationUser> CreateAsync(ApplicationUser user, string password)
        {
            await _userManager.CreateAsync(user, password);
            return user;
        }

        public async Task AssignUserToRoleAsync(string userName, string roleName)
        {
            // Verify the role exists, if not create it. 
            var role = await _roleManager.RoleExistsAsync(roleName);
            if (!role)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Get the user. If the user exists, and if the user doesn't already have
            //      the role, add the role.
            var user = await ReadAsync(userName);
            if (user != null)
            {
                if (!user.HasRole(roleName))
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

        }
    }
}
