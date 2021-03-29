using JordanDeBordProject2.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JordanDeBordProject2.Services
{
    /// <summary>
    /// ApplicationDbContext class acts as the bridge to our database.
    /// We interact with our database through this class. 
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        /// <summary>
        /// Constructor for ApplicationDbContext.
        /// </summary>
        /// <param name="options">Options to be passed to the superconstructor. These are
        ///     used to link to the database using the connectionstring from appsettings.json. </param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Represents the associated table in our databse for Genres.
        /// </summary>
        public DbSet<Genre> Genres {get; set;}

        /// <summary>
        /// Represents the table in the database for Movies.
        /// </summary>
        public DbSet<Movie> Movies {get; set;}
        
        /// <summary>
        /// Represents the table in the database for MovieGenres. This table is the
        /// associative entity between the Movies and Genres tables.
        /// </summary>
        public DbSet<MovieGenre> MovieGenres {get; set;}
        
        /// <summary>
        /// Represents the Profile table in the database.
        /// </summary>
        public DbSet<Profile> Profiles {get; set;}
        
        /// <summary>
        /// Represents the PaidMovies table in the database. This table is the
        /// associative entity between the Profiles and Movies tables. 
        /// </summary>
        public DbSet<PaidMovie> PaidMovies {get; set;}

}
}
