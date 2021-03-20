using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public interface IProfileRepository
    {
        Task<Profile> ReadAsync(int id);

        Task<ICollection<Profile>> ReadAllAsync();

        Task<Profile> CreateAsyc(Profile profile);

        Task TaskUpdateAsyc(Profile profile);

        Task DeleteAsync(int id);

        Task AddPurchasedMovie(int profileId, int movieId);

        Task<bool> CheckProfile(string userId);
    }
}
