using JordanDeBordProject2.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JordanDeBordProject2.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Genre> Genres {get; set;}
        public DbSet<Movie> Movies {get; set;}
        public DbSet<MovieGenre> MovieGenres {get; set;}
        public DbSet<Profile> Profiles {get; set;}
        public DbSet<PaidMovie> PaidMovies {get; set;}

}
}
