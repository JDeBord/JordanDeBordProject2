using JordanDeBordProject2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    /// <summary>
    /// Movie Repository, which adds a level of abstraction to our application. We implement our interface 
    /// here. It contains our CRUD methods, and interacts with the database through the ApplicationDBContext 
    /// and DbSets. 
    /// </summary>
    public class DbMovieRepository : IMovieRepository
    {
        private ApplicationDbContext _database;

        /// <summary>
        /// Cosntructor for our Movie Repository, where we inject our ApplicationDbContext.
        /// </summary>
        /// <param name="database">ApplicationDbContext with which we interact with our database.</param>
        public DbMovieRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Adds a Genre to a Movie. In doing so, a MovieGenre is created and added to the movie and genre.  
        /// </summary>
        /// <param name="movieId">Id of Movie to add Genre to.</param>
        /// <param name="genre">Genre to add to the Movie.</param>
        public async Task AddGenreAsync(int movieId, Genre genre)
        {
            // Get the movie to add the Genre to.
            var movie = await ReadAsync(movieId);

            // If that movie does not already have the genre, add it. 
            if (!(movie.MovieGenres.Any(mg => mg.Genre.Id == genre.Id)))
            {
                var movieGenre = new MovieGenre
                {
                    Movie = movie,
                    Genre = genre
                };

                movie.MovieGenres.Add(movieGenre);
                genre.GenreMovies.Add(movieGenre);

                await _database.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes the genre from the movie with the provided Id.
        /// </summary>
        /// <param name="movieId">Id of the Movie to remove the Genre from.</param>
        /// <param name="genre">Genre to be removed from the Movie.</param>
        public async Task RemoveGenreAsync(int movieId, Genre genre)
        {
            var movie = await ReadAsync(movieId);

            // If the movie has the movie genre, remove it. 
            if (movie.MovieGenres.Any(mg => mg.Genre.Id == genre.Id))
            {
                var movieGenre = await _database.MovieGenres
                                    .FirstOrDefaultAsync(mg => mg.GenreId == genre.Id && mg.MovieId == movie.Id);

                // Make sure it returned the MovieGenre correctly, and remove it. 
                if (movieGenre != null)
                {
                    _database.MovieGenres.Remove(movieGenre);
                    await _database.SaveChangesAsync();
                }
            }
        }
        
        /// <summary>
        /// Add the Movie to the database.
        /// </summary>
        /// <param name="movie">Movie to be added to the database.</param>
        /// <returns>A Movie that was added to the database.</returns>
        public async Task<Movie> CreateAsyc(Movie movie)
        {
            await _database.Movies.AddAsync(movie);
            await _database.SaveChangesAsync();
            
            return movie;
        }

        /// <summary>
        /// Removes the Movie with the provided Id from the database.
        /// </summary>
        /// <param name="movieId">Id of the movie to be removed from the database.</param>
        public async Task DeleteAsync(int movieId)
        {
            var movieToDelete = await ReadAsync(movieId);

            _database.Movies.Remove(movieToDelete);
            
            await _database.SaveChangesAsync();

        }

        /// <summary>
        /// Method to get a list of all Movies from the database and their Genres.
        /// </summary>
        /// <returns>A list containing all Movies from the database and their Genres.</returns>
        public async Task<ICollection<Movie>> ReadAllAsync()
        {
            var movies = await _database.Movies
                                    .Include(mg => mg.MovieGenres)
                                    .ThenInclude(g => g.Genre)
                                    .ToListAsync();

            return movies;
        }

        /// <summary>
        /// Method to get the Movie (and its Genres) with the provided Id.
        /// </summary>
        /// <param name="movieId">Id of the Movie to return.</param>
        /// <returns>The Movie with the associated Id.</returns>
        public async Task<Movie> ReadAsync(int movieId)
        {
            var movie = await _database.Movies
                .Include(mg => mg.MovieGenres)
                .ThenInclude(g => g.Genre)
                .FirstOrDefaultAsync(movie => movie.Id == movieId);

            return movie;
        }

        /// <summary>
        /// Method to get the Movie from the database with the provided Title. This is used
        /// by the Initializer to make sure our 10 default movies exist.
        /// </summary>
        /// <param name="movieTitle">Title of Movie to be returned.</param>
        /// <returns>First Movie from the database with that Title.</returns>
        public async Task<Movie> ReadByNameAsync(string movieTitle)
        {
            var movie = await _database.Movies.FirstOrDefaultAsync(m => m.Title == movieTitle);

            return movie;
        }

        /// <summary>
        /// Updates the Movie in the database with the provided information.
        /// </summary>
        /// <param name="movie">Movie containing the new information for our Movie.</param>
        public async Task UpdateAsyc(Movie movie)
        {
            var movieToUpdate = await ReadAsync(movie.Id);

            movieToUpdate.Title = movie.Title;
            movieToUpdate.Year = movie.Year;
            movieToUpdate.LengthInMinutes = movie.LengthInMinutes;
            movieToUpdate.Price = movie.Price;
            movieToUpdate.IMDB_URL = movie.IMDB_URL;

            await _database.SaveChangesAsync();
        }
    }
}
