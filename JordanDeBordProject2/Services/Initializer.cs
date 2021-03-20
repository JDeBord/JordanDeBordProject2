using JordanDeBordProject2.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public class Initializer
    {
        private readonly ApplicationDbContext _database;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Initializer
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

        public async Task SeedUsersAsync()
        {
            _database.Database.EnsureCreated();

            if (!_database.Roles.Any(role => role.Name == "Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }

            if (!_database.Roles.Any(role => role.Name == "Movie Connoisseur"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Movie Connoisseur" });
            }

            if (!_database.Users.Any(user => user.UserName == "admin@test.com"))
            {
                var user = new ApplicationUser
                {
                    Email = "admin@test.com",
                    UserName = "admin@test.com",
                    FirstName = "Admin",
                    LastName = "Admin"
                };
                await _userManager.CreateAsync(user, "Pass123!");
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        /*
        public async Task SeedMoviesAsync()
        { 
        
        }
        */

    }
}
