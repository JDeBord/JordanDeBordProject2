using JordanDeBordProject2.Models.Entities;
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

        public Task<Genre> CreateAsyc(Genre genre)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int GenreId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Genre>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Genre> ReadAsync(int GenreId)
        {
            throw new NotImplementedException();
        }

        public Task TaskUpdateAsyc(Genre genre)
        {
            throw new NotImplementedException();
        }
    }
}
