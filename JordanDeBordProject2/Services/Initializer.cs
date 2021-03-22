using JordanDeBordProject2.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Services
{
    public class Initializer
    {
        private readonly ApplicationDbContext _database;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IGenreRepository _genreRepo;
        private readonly IMovieRepository _movieRepo;

        public Initializer
            (
                ApplicationDbContext database,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager,
                IGenreRepository genreRepo,
                IMovieRepository movieRepo
            )
        {
            _database = database;
            _userManager = userManager;
            _roleManager = roleManager;
            _genreRepo = genreRepo;
            _movieRepo = movieRepo;
        }

        public async Task SeedUsersAsync()
        {
            _database.Database.EnsureCreated();

            if (!_database.Roles.Any(role => role.Name == "Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }

            if (!_database.Roles.Any(role => role.Name == "Movie Connoisseur"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Movie Connoisseur" });
            }

            if (!_database.Users.Any(user => user.UserName == "admin@test.com"))
            {
                var user = new ApplicationUser
                {
                    Email = "admin@test.com",
                    UserName = "admin@test.com",
                    FirstName = "Admin",
                    LastName = "Admin"
                };
                await _userManager.CreateAsync(user, "Pass123!");
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        public async Task SeedGenresAsync()
        {
            if (!_database.Genres.Any(genre => genre.Name == "Action")) 
            {
                await _genreRepo.CreateAsyc(new Genre { Name = "Action" });
            };

            if (!_database.Genres.Any(genre => genre.Name == "Drama"))
            {
                await _genreRepo.CreateAsyc(new Genre { Name = "Drama" });
            };

            if (!_database.Genres.Any(genre => genre.Name == "Horror"))
            {
                await _genreRepo.CreateAsyc(new Genre { Name = "Horror" });
            };

            if (!_database.Genres.Any(genre => genre.Name == "Documentary"))
            {
                await _genreRepo.CreateAsyc(new Genre { Name = "Documentary" });
            };

            if (!_database.Genres.Any(genre => genre.Name == "Romance"))
            {
                await _genreRepo.CreateAsyc(new Genre { Name = "Romance" });
            };

            if (!_database.Genres.Any(genre => genre.Name == "Crime"))
            {
                await _genreRepo.CreateAsyc(new Genre { Name = "Crime" });
            };

            if (!_database.Genres.Any(genre => genre.Name == "Musical"))
            {
                await _genreRepo.CreateAsyc(new Genre { Name = "Musical" });
            };

            if (!_database.Genres.Any(genre => genre.Name == "Adventure"))
            {
                await _genreRepo.CreateAsyc(new Genre { Name = "Adventure" });
            };

            if (!_database.Genres.Any(genre => genre.Name == "Thriller"))
            {
                await _genreRepo.CreateAsyc(new Genre { Name = "Thriller" });
            };

            if (!_database.Genres.Any(genre => genre.Name == "Comedy"))
            {
                await _genreRepo.CreateAsyc(new Genre { Name = "Comedy" });
            };
        }

        public async Task SeedMoviesAsync()
        {
            if (!_database.Movies.Any(movie => movie.Title == "The Lord of the Rings: The Fellowship of the Ring"))
            {
                await _movieRepo.CreateAsyc(new Movie
                {
                    Title = "The Lord of the Rings: The Fellowship of the Ring",
                    Year = 2001,
                    LengthInMinutes = 178,
                    Price = 10.00,
                    IMDB_URL = "https://www.imdb.com/title/tt0120737"
                });
            }

            if (!_database.Movies.Any(movie => movie.Title == "The Godfather"))
            {
                await _movieRepo.CreateAsyc(new Movie
                {
                    Title = "The Godfather",
                    Year = 1972,
                    LengthInMinutes = 175,
                    Price = 7.50,
                    IMDB_URL = "https://www.imdb.com/title/tt0068646"
                });
            }

            if (!_database.Movies.Any(movie => movie.Title == "Forrest Gump"))
            {
                await _movieRepo.CreateAsyc(new Movie
                {
                    Title = "Forrest Gump",
                    Year = 1994,
                    LengthInMinutes = 142,
                    Price = 8.99,
                    IMDB_URL = "https://www.imdb.com/title/tt0109830"
                });
            }

            if (!_database.Movies.Any(movie => movie.Title == "The Silence of the Lambs"))
            {
                await _movieRepo.CreateAsyc(new Movie
                {
                    Title = "The Silence of the Lambs",
                    Year = 1991,
                    LengthInMinutes = 118,
                    Price = 7.99,
                    IMDB_URL = "https://www.imdb.com/title/tt0102926"
                });
            }

            if (!_database.Movies.Any(movie => movie.Title == "The Lion King"))
            {
                await _movieRepo.CreateAsyc(new Movie
                {
                    Title = "The Lion King",
                    Year = 1994,
                    LengthInMinutes = 88,
                    Price = 5.00,
                    IMDB_URL = "https://www.imdb.com/title/tt0110357"
                });
            }

            if (!_database.Movies.Any(movie => movie.Title == "Gladiator"))
            {
                await _movieRepo.CreateAsyc(new Movie
                {
                    Title = "Gladiator",
                    Year = 2000,
                    LengthInMinutes = 155,
                    Price = 4.99,
                    IMDB_URL = "https://www.imdb.com/title/tt0172495"
                });
            }

            if (!_database.Movies.Any(movie => movie.Title == "Braveheart"))
            {
                await _movieRepo.CreateAsyc(new Movie
                {
                    Title = "Braveheart",
                    Year = 1995,
                    LengthInMinutes = 178,
                    Price = 6.50,
                    IMDB_URL = "https://www.imdb.com/title/tt0112573"
                });
            }

            if (!_database.Movies.Any(movie => movie.Title == "Amadeus"))
            {
                await _movieRepo.CreateAsyc(new Movie
                {
                    Title = "Amadeus",
                    Year = 1984,
                    LengthInMinutes = 160,
                    Price = 4.99,
                    IMDB_URL = "https://www.imdb.com/title/tt0086879"
                });
            }

            if (!_database.Movies.Any(movie => movie.Title == "Hamilton"))
            {
                await _movieRepo.CreateAsyc(new Movie
                {
                    Title = "Hamilton",
                    Year = 2020,
                    LengthInMinutes = 160,
                    Price = 19.99,
                    IMDB_URL = "https://www.imdb.com/title/tt8503618"
                });
            }

            if (!_database.Movies.Any(movie => movie.Title == "1917"))
            {
                await _movieRepo.CreateAsyc(new Movie
                {
                    Title = "1917",
                    Year = 2019,
                    LengthInMinutes = 119,
                    Price = 15.99,
                    IMDB_URL = "https://www.imdb.com/title/tt8579674"
                });
            }
        }

        public async Task SeedMovieGenresAsync() 
        {
            Movie movie = null;
            Genre genre = null;
            // Genres for LOTR.
            movie = await _movieRepo.ReadByNameAsync("The Lord of the Rings: The Fellowship of the Ring");
            genre = await _genreRepo.ReadByNameAsync("Action");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            genre = await _genreRepo.ReadByNameAsync("Adventure");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }
            // Genres for The Godfather.
            movie = await _movieRepo.ReadByNameAsync("The Godfather");
            genre = await _genreRepo.ReadByNameAsync("Drama");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            genre = await _genreRepo.ReadByNameAsync("Crime");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            // Genres for Forrest Gump.
            movie = await _movieRepo.ReadByNameAsync("Forrest Gump");
            genre = await _genreRepo.ReadByNameAsync("Drama");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            genre = await _genreRepo.ReadByNameAsync("Romance");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            // Genres for Silence of the Lambs
            movie = await _movieRepo.ReadByNameAsync("The Silence of the Lambs");
            genre = await _genreRepo.ReadByNameAsync("Drama");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            genre = await _genreRepo.ReadByNameAsync("Crime");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            genre = await _genreRepo.ReadByNameAsync("Thriller");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            // Genres for The Lion King.
            movie = await _movieRepo.ReadByNameAsync("The Lion King");
            genre = await _genreRepo.ReadByNameAsync("Drama");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            genre = await _genreRepo.ReadByNameAsync("Adventure");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            // Genres for Gladiator.
            movie = await _movieRepo.ReadByNameAsync("Gladiator");
            genre = await _genreRepo.ReadByNameAsync("Action");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            genre = await _genreRepo.ReadByNameAsync("Adventure");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }
            // Genres for Braveheart.
            movie = await _movieRepo.ReadByNameAsync("Braveheart");
            genre = await _genreRepo.ReadByNameAsync("Drama");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            // Genres for Amadeus.
            movie = await _movieRepo.ReadByNameAsync("Amadeus");
            genre = await _genreRepo.ReadByNameAsync("Drama");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            // Genres for Hamilton.
            movie = await _movieRepo.ReadByNameAsync("Hamilton");
            genre = await _genreRepo.ReadByNameAsync("Musical");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }

            // Genres for 1917.
            movie = await _movieRepo.ReadByNameAsync("1917");
            genre = await _genreRepo.ReadByNameAsync("Thriller");
            if (movie != null && genre != null)
            {
                if (!_database.MovieGenres.Any(mg => mg.Movie == movie && mg.Genre == genre))
                {
                    await _movieRepo.AddGenreAsync(movie.Id, genre);
                }
            }
        }
        

    }
}
