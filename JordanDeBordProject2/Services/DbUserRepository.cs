using JordanDeBordProject2.Models.Entities;
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
        public ApplicationUser Read(string userName)
        {
            return _database.Users.FirstOrDefault(u => u.UserName == userName);
        }
    }
}
