using JordanDeBordProject2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    /// <summary>
    /// Db User Repository which adds a level of abstraction to our application. This class
    /// is the instantiation of our Interface. Our Read and Update functionality are here for users.
    /// This uses the ApplicationDbContext and DBSets to access the database.
    /// </summary>
    public class DbUserRepository : IUserRepository
    {
        private ApplicationDbContext _database;

        /// <summary>
        /// Constructor for our Repository, in which we inject our ApplicationDbContext.
        /// </summary>
        /// <param name="database">ApplicationDbContext with which we interact with our database.</param>
        public DbUserRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Read the user from our database using their UserName.
        /// </summary>
        /// <param name="userName">UserName of User to return.</param>
        /// <returns>ApplicationUser associated with that UserName.</returns>
        public async Task<ApplicationUser> ReadAsync(string userName)
        {
            var user = await _database.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            return user;
        }

        /// <summary>
        /// Updates the User's first and last name in our Database.
        /// </summary>
        /// <param name="user">ApplicationUser containing new information to update.</param>
        public async Task UpdateAsync(ApplicationUser user)
        {
            var userToUpdate = await ReadAsync(user.UserName);

            if (userToUpdate != null)
            {
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;

                await _database.SaveChangesAsync();
            }
        }
    }
}
