using JordanDeBordProject2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public class DbGenreRepository : IGenreRepository
    {
        private ApplicationDbContext _database;

        public DbGenreRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        public async Task<Genre> CreateAsyc(Genre genre)
        {
            await _database.Genres.AddAsync(genre);
            await _database.SaveChangesAsync();

            return genre;
        }

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
