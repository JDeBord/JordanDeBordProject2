using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    /// <summary>
    /// Interface for our Profile repository, which contains CRUD method headers
    /// to be implemented in the Repository. 
    /// </summary>
    public interface IProfileRepository
    {
        Task<Profile> ReadAsync(int profileId);

        Task<ICollection<Profile>> ReadAllAsync();

        Task<Profile> CreateAsyc(string applicationUserId, Profile profile);

        Task UpdateAsyc(Profile profile);

        Task DeleteAsync(int profileId);

        Task AddPaidMovie(int profileId, Movie movie);

        Task<ICollection<PaidMovie>> GetPaidMoviesAsync(int profileId);

        Task<bool> CheckProfile(string applicationUserId);
        
        Task<Profile> ReadByUserAsync(string applicationUserId);

        Task UpdateWatchedCountAsync(int profileId, int movieId);
    }
}
