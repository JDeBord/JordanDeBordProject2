using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public interface IProfileRepository
    {
        Task<Profile> ReadAsync(int profileId);

        ICollection<Profile> ReadAllAsync();

        Task<Profile> CreateAsyc(Profile profile);

        Task TaskUpdateAsyc(Profile profile);

        Task DeleteAsync(int profileId);

        Task AddPaidMovie(int profileId, Movie movie);

        Task<bool> CheckProfile(string userId);
    }
}
