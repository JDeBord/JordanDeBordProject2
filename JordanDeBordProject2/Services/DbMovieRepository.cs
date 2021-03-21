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

        public async Task AddGenreAsync(int movieId, int genreId)
        {
            var movie = await ReadAsync(movieId);
            var genre = await ReadGenreAsync(genreId);

            if (movie != null && genre != null)
            {
                movie.MovieGenres.Add(genre);
                genre.GenreMovies.Add(movie);
            }
        }

        public async Task<Movie> CreateAsyc(Movie movie)
        {
            await _database.Movies.AddAsync(movie);
            _database.SaveChanges();
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
                .ToList();

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

        public async Task UpdateAsyc(Movie movie)
        {
            var movieToUpdate = await ReadAsync(movie.Id);

            movieToUpdate.Title = movie.Title;
            movieToUpdate.Year = movie.Year;
            movieToUpdate.Price = movie.Price;
            movieToUpdate.IMDB_URL = movie.IMDB_URL;

            await _database.SaveChangesAsync();
        }
        public async Task<Genre> ReadGenreAsync(int genreId)
        {
            var genre = await _database.Genres
                .FirstOrDefaultAsync(genre => genre.Id == genreId);

            return genre;
        }
    }
}
