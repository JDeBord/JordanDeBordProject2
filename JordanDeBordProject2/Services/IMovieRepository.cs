using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public interface IMovieRepository
    {
        Task<Movie> ReadAsync(int id);
        
        Task<ICollection<Movie>> ReadAllAsync();

        Task<Movie> CreateAsyc(Movie movie);
        
        Task TaskUpdateAsyc(Movie movie);
        
        Task DeleteAsync(int id);

        Task AddGenreAsync(int movieId, int genreId);
    }
}
