using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public interface IMovieRepository
    {
        Task<Movie> ReadAsync(int movieId);
        
        Task<ICollection<Movie>> ReadAllAsync();

        Task<Movie> CreateAsyc(Movie movie);
        
        Task UpdateAsyc(Movie movie);
        
        Task DeleteAsync(int movieId);

        Task AddGenreAsync(int movieId, int genreId);
    }
}
