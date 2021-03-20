using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public class DbProfileRepository : IProfileRepository
    {
        private ApplicationDbContext _database;

        public DbProfileRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        public Task AddPurchasedMovie(int profileId, int movieId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckProfile(string userId)
        {
            await foreach (var profile in _database.Profiles)
            {
                if (profile.ApplicationUserId == userId)
                {
                    return true;
                }
            }

            return false;
        }

        public Task<Profile> CreateAsyc(Profile profile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Profile>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Profile> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task TaskUpdateAsyc(Profile profile)
        {
            throw new NotImplementedException();
        }
    }
}
