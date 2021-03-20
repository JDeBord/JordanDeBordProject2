using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public interface IGenreRepository
    {
        Task<Genre> ReadAsync(int id);

        Task<ICollection<Genre>> ReadAllAsync();

        Task<Genre> CreateAsyc(Genre genre);

        Task TaskUpdateAsyc(Genre genre);

        Task DeleteAsync(int id);
    }
}
