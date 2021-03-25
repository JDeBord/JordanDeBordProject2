using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public interface IUserRepository
    {
        Task<ApplicationUser> ReadAsync(string userName);
        Task<ApplicationUser> ReadByIdAsync(string userId);

        Task UpdateAsync(ApplicationUser user);
    }
}
