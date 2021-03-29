using JordanDeBordProject2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    /// <summary>
    /// Profile Repository, which adds a level of abstraction to our application. This contains our
    /// CRUD methods, and uses the ApplicationDbContext and DbSets to access the database.
    /// </summary>
    public class DbProfileRepository : IProfileRepository
    {
        private ApplicationDbContext _database;

        /// <summary>
        /// Constructor for our repository, where we inject our ApplicationDbContext.
        /// </summary>
        /// <param name="database">ApplicationDbContext with which we interact with our database.</param>
        public DbProfileRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Adds a PaidMovie, which represents the purchase of a Movie. 
        /// </summary>
        /// <param name="profileId">Id of Profile to add the purchased Movie to.</param>
        /// <param name="movie">Movie purchased by that Profile.</param>
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

            // Get the movie we just added, to update the price. (We store it in case future prices change)
            var newPaidMovie = await _database.PaidMovies
                                    .Where(m => m.MovieId == movie.Id)
                                    .Where(p => p.ProfileId == profileId)
                                    .FirstOrDefaultAsync();
            // As long as we returned it correctly, update the price to the current sale price.
            // This allows us to change movie prices and still save the sold price at the sale date. 
            if (newPaidMovie != null)
            {
                newPaidMovie.SalePrice = movie.Price;

                await _database.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Adds the profile to the database.
        /// </summary>
        /// <param name="ApplicationUserId">ApplicationUserId to assign to the Profile.</param>
        /// <param name="profile">Profile to be added to the database.</param>
        /// <returns>Profile that was added to the database.</returns>
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

        /// <summary>
        /// Removes the Profile with the provided Id from the database.
        /// </summary>
        /// <param name="profileId">Id of the profile to be removed.</param>
        public async Task DeleteAsync(int profileId)
        {
            var profileToDelete = await ReadAsync(profileId);

            _database.Profiles.Remove(profileToDelete);

            await _database.SaveChangesAsync();
        }

        /// <summary>
        /// Method to return the Profile from the database with the provided Profile Id.
        /// </summary>
        /// <param name="profileId">Profile Id to be returned from the database.</param>
        /// <returns>Profile with the provided Id.</returns>
        public async Task<Profile> ReadAsync(int profileId)
        {
            var profile = await _database.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
            return profile;
        }

        /// <summary>
        /// Updates the profile in the database with the provided information.
        /// </summary>
        /// <param name="profile">Profile containing the updated information to be stored.</param>
        public async Task UpdateAsyc(Profile profile)
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

        /// <summary>
        /// Method to return the Profile with the associated ApplicationUserId, also include the paid movies.
        /// </summary>
        /// <param name="applicationUserId">ApplicationUserId of the Profile to return.</param>
        /// <returns>Profile with that ApplicationUserId.</returns>
        public async Task<Profile> ReadByUserAsync(string applicationUserId) 
        {
            var profile = await _database.Profiles.Include(pm=> pm.PaidMovies).FirstOrDefaultAsync(p => p.ApplicationUserId == applicationUserId);

            return profile;
        }

        /// <summary>
        /// Method to return a list of all the PaidMovies (and Movies) of the Profile with the Id provided.
        /// </summary>
        /// <param name="profileId">Id of Profile to return PaidMovies for.</param>
        /// <returns>List of PaidMovies for the provided Profile Id.</returns>
        public async Task<ICollection<PaidMovie>> GetPaidMoviesAsync(int profileId) 
        {
            var query = await _database.PaidMovies.Include(m => m.Movie).ToListAsync();

            var movies = query.Where(p => p.ProfileId == profileId).ToList();

            return movies;
        }

        /// <summary>
        /// Updates the number of times watched for the PaidMovie between a Profile and Movie.
        /// Increases the number by one. 
        /// </summary>
        /// <param name="profileId">Id of Profile associated with the PaidMovie.</param>
        /// <param name="movieId">Id of Movie associated with the PaidMovie.</param>
        public async Task UpdateWatchedCountAsync(int profileId, int movieId)
        {
            var paidMovie = await _database.PaidMovies.Where(p => p.ProfileId == profileId)
                                .Where(m => m.MovieId == movieId).FirstOrDefaultAsync();

            if (paidMovie != null)
            {
                paidMovie.TimesWatched += 1;
                await _database.SaveChangesAsync();
            }
        }
    }
}
