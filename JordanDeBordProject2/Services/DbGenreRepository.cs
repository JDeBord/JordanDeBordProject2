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
        public async Task<Genre> CreateAsyc(Genre genre)
        {
            await _database.Genres.AddAsync(genre);
            await _database.SaveChangesAsync();
            
            return genre;
        }

        /// <summary>
        /// Removes the genre from the database. 
        /// </summary>
        /// <param name="genreId">Id of the Genre to be deleted.</param>
        public async Task DeleteAsync(int genreId)
        {
            var genreToDelete = await ReadAsync(genreId);
            _database.Remove(genreToDelete);

            await _database.SaveChangesAsync();
        }

        public async Task<ICollection<Genre>> ReadAllAsync()
        {
            var genres = await _database.Genres.ToListAsync();
            return genres;
        }

        public async Task<Genre> ReadAsync(int genreId)
        {
            var genre = await _database.Genres.FirstOrDefaultAsync(g => g.Id == genreId);

            return genre;
        }

        public async Task<Genre> ReadByNameAsync(string genreName)
        {
            var genre = await _database.Genres.FirstOrDefaultAsync(g => g.Name == genreName);

            return genre;
        }

        public async Task UpdateAsyc(Genre genre)
        {
            var genreToUpdate = await ReadAsync(genre.Id);

            genreToUpdate.Name = genre.Name;

            await _database.SaveChangesAsync();
        }
    }
}
