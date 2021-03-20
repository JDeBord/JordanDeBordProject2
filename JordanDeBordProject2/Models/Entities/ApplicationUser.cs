using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        public Profile Profile { get; set; }

        [NotMapped]
        public ICollection<string> Roles { get; set; }

        public bool HasRole(string roleName)
        {
            return Roles.Any(role => role == roleName);
        }
    }
}
