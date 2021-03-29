using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    /// <summary>
    /// Interface for our Genre Repository with method headers for CRUD operations
    /// which are to be implemented in our repository. 
    /// </summary>
    public interface IGenreRepository
    {
        Task<Genre> ReadAsync(int genreId);

        Task<Genre> ReadByNameAsync(string genreName);

        Task<ICollection<Genre>> ReadAllAsync();

        Task<Genre> CreateAsyc(Genre genre);

        Task UpdateAsyc(Genre genre);

        Task DeleteAsync(int genreId);
    }
}
