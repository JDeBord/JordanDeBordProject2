using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser> ReadAsync(string userName);

        Task<ApplicationUser> CreateAsync(ApplicationUser user, string password);
        
        Task AssignUserToRoleAsync(string userName, string roleName);
    }
}
