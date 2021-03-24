using JordanDeBordProject2.Models.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task AddPaidMovie(int profileId, Movie movie)
        {
            var profile = await ReadAsync(profileId);
            var paidMovie = new PaidMovie
            {
                Profile = profile,
                Movie = movie
            };

            profile.PaidMovies.Add(paidMovie);
            movie.PaidMovies.Add(paidMovie);

            await _database.SaveChangesAsync();
        }

        

        public async Task<Profile> CreateAsyc(string ApplicationUserId, Profile profile)
        {
            var user = await _database.Users.FirstOrDefaultAsync(u => u.Id == ApplicationUserId);

            if (user != null)
            {
                profile.ApplicationUser = user;
                user.Profile = profile;
                await _database.Profiles.AddAsync(profile);

                await _database.SaveChangesAsync();
            }

            return profile;
        }

        public async Task DeleteAsync(int profileId)
        {
            var profileToDelete = await ReadAsync(profileId);

            _database.Profiles.Remove(profileToDelete);

            await _database.SaveChangesAsync();
        }

        public async Task<ICollection<Profile>> ReadAllAsync()
        {
            return await _database.Profiles.ToListAsync();
        }

        public async Task<Profile> ReadAsync(int profileId)
        {
            var profile = await _database.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
            return profile;
        }

        public async Task TaskUpdateAsyc(Profile profile)
        {
            var profileToUpdate = await ReadAsync(profile.Id);

            profileToUpdate.CCNum = profile.CCNum;
            profileToUpdate.CCExp = profile.CCExp;
            profileToUpdate.AddLine1 = profile.AddLine1;
            profileToUpdate.AddLine2 = profile.AddLine2;
            profileToUpdate.City = profile.City;
            profileToUpdate.State = profile.State;
            profileToUpdate.ZIPCode = profile.ZIPCode;

            await _database.SaveChangesAsync();
        }

        public async Task<bool> CheckProfile(string applicationUserId) 
        {
            var profile = await ReadByUserAsync(applicationUserId);

            if (profile == null)
            {
                return false;
            }

            return true;
        }

        public async Task<Profile> ReadByUserAsync(string applicationUserId) 
        {
            var profile = await _database.Profiles.FirstOrDefaultAsync(p => p.ApplicationUserId == applicationUserId);

            return profile;
        }
    }
}
