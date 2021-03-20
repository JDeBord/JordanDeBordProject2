using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public class DbMovieRepository : IMovieRepository
    {
        private ApplicationDbContext _database;

        public DbMovieRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        public Task AddGenreAsync(int movieId, int genreId)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> CreateAsyc(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Movie>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Movie> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task TaskUpdateAsyc(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
