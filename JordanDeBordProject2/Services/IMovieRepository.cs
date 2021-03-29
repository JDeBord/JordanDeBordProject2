using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    /// <summary>
    /// Interface for our Movie Repository which contains CRUD method headers to be implemented
    /// in our Repository.
    /// </summary>
    public interface IMovieRepository
    {
        Task<Movie> ReadAsync(int movieId);

        Task<Movie> ReadByNameAsync(string movieTitle);
        
        Task<ICollection<Movie>> ReadAllAsync();

        Task<Movie> CreateAsyc(Movie movie);
        
        Task UpdateAsyc(Movie movie);
        
        Task DeleteAsync(int movieId);

        Task AddGenreAsync(int movieId, Genre genre);

        Task RemoveGenreAsync(int movieId, Genre genre);

        Task<ICollection<MovieGenre>> ReadAllMovieGenreAsync(int genreId);
    }
}
