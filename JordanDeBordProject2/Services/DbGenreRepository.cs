using JordanDeBordProject2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    /// <summary>
    /// Genre repository which implements our interface and adds a level of abstraction to our application.
    /// Our CRUD methods are located here. It interacts with the database through the ApplicationDbContext and DbSets.
    /// </summary>
    public class DbGenreRepository : IGenreRepository
    {
        private ApplicationDbContext _database;

        /// <summary>
        /// Constructor for our Genre Repository, where we inject our ApplicationDbContext.
        /// </summary>
        /// <param name="database">ApplicationDbContext with which we interact with our database.</param>
        public DbGenreRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Adds the Genre provided to the database. 
        /// </summary>
        /// <param name="genre">Genre to be added to the database.</param>
        /// <returns>Genre that was added to the database.</returns>
        public async Task<Genre> CreateAsyc(Genre genre)
        {
            await _database.Genres.AddAsync(genre);
            await _database.SaveChangesAsync();
            
            return genre;
        }

        /// <summary>
        /// Removes the genre from the database. 
        /// </summary>
        /// <param name="genreId">Id of the Genre to be removed.</param>
        public async Task DeleteAsync(int genreId)
        {
            var genreToDelete = await ReadAsync(genreId);
            _database.Remove(genreToDelete);

            await _database.SaveChangesAsync();
        }

        /// <summary>
        /// Method to read all Genres from the database.
        /// </summary>
        /// <returns>A List of all Genres from the database.</returns>
        public async Task<ICollection<Genre>> ReadAllAsync()
        {
            var genres = await _database.Genres.ToListAsync();
            return genres;
        }

        /// <summary>
        /// Method to get the first genre with the provided Id.
        /// </summary>
        /// <param name="genreId">Id of the genre to be returned.</param>
        /// <returns>Genre with the provided Id.</returns>
        public async Task<Genre> ReadAsync(int genreId)
        {
            var genre = await _database.Genres.FirstOrDefaultAsync(g => g.Id == genreId);

            return genre;
        }

        /// <summary>
        /// Returns the first genre from the database with the associated name. This is used by 
        /// our Initializer to make sure the Genre is associated with the movie. 
        /// </summary>
        /// <param name="genreName">Name of genre to be returned.</param>
        /// <returns>First genre from the database with that name.</returns>
        public async Task<Genre> ReadByNameAsync(string genreName)
        {
            var genre = await _database.Genres.FirstOrDefaultAsync(g => g.Name == genreName);

            return genre;
        }

        /// <summary>
        /// Updates the genre in the database.
        /// </summary>
        /// <param name="genre">Genre containing information to be updated.</param>
        public async Task UpdateAsyc(Genre genre)
        {
            var genreToUpdate = await ReadAsync(genre.Id);

            genreToUpdate.Name = genre.Name;

            await _database.SaveChangesAsync();
        }
    }
}
