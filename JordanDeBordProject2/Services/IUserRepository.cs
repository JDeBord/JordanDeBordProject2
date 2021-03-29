using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    /// <summary>
    /// Interface for our User Repository. It contains method headers for read 
    /// and update operations for users which are to be implemented in the Repository.
    /// </summary>
    public interface IUserRepository
    {
        Task<ApplicationUser> ReadAsync(string userName);

        Task UpdateAsync(ApplicationUser user);
    }
}
