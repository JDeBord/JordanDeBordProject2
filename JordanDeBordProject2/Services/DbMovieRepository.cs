using JordanDeBordProject2.Models.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task AddGenreAsync(int movieId, Genre genre)
        {
            var movie = await ReadAsync(movieId);

            var movieGenre = new MovieGenre
            {
                Movie = movie,
                Genre = genre
            };
            movie.MovieGenres.Add(movieGenre);
            genre.GenreMovies.Add(movieGenre);

            await _database.SaveChangesAsync();
        }

        public async Task<Movie> CreateAsyc(Movie movie)
        {
            await _database.Movies.AddAsync(movie);
            await _database.SaveChangesAsync();
            return movie;
        }

        public async Task DeleteAsync(int movieId)
        {
            var movieToDelete = await ReadAsync(movieId);
            _database.Movies.Remove(movieToDelete);
            await _database.SaveChangesAsync();

        }

        public async Task<ICollection<Movie>> ReadAllAsync()
        {
            var movies = await _database.Movies
                                    .Include(mg => mg.MovieGenres)
                                    .ThenInclude(g => g.Genre)
                                    .ToListAsync();

            return movies;
        }

        public async Task<Movie> ReadAsync(int movieId)
        {
            var movie = await _database.Movies
                .Include(mg => mg.MovieGenres)
                .ThenInclude(g => g.Genre)
                .FirstOrDefaultAsync(movie => movie.Id == movieId);

            return movie;
        }

        public async Task<Movie> ReadByNameAsync(string movieTitle)
        {
            var movie = await _database.Movies.FirstOrDefaultAsync(m => m.Title == movieTitle);

            return movie;
        }

        public async Task UpdateAsyc(Movie movie)
        {
            var movieToUpdate = await ReadAsync(movie.Id);

            movieToUpdate.Title = movie.Title;
            movieToUpdate.Year = movie.Year;
            movieToUpdate.Price = movie.Price;
            movieToUpdate.IMDB_URL = movie.IMDB_URL;

            await _database.SaveChangesAsync();
        }
    }
}
