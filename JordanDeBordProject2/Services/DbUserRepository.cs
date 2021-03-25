using JordanDeBordProject2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public class DbUserRepository : IUserRepository
    {
        private ApplicationDbContext _database;

        public DbUserRepository(ApplicationDbContext database)
        {
            _database = database;
        }
        public async Task<ApplicationUser> ReadAsync(string userName)
        {
            var user = await _database.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            return user;
        }

        public async Task<ApplicationUser> ReadByIdAsync(string userId)
        {
            var user = await _database.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            var userToUpdate = await ReadByIdAsync(user.Id);

            if (userToUpdate != null)
            {
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;

                await _database.SaveChangesAsync();
            }
        }
    }
}
