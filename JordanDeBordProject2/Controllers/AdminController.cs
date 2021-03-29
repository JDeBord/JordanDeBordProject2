using JordanDeBordProject2.Models.ViewModels;
using JordanDeBordProject2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Controllers
{
    /// <summary>
    /// Admin Controller, which handles client requests and directs them to the appropriate action method and then sends
    /// the response to user. It handles requests from clients to /admin/{action} where the action is the name of the method below.
    /// Non-logged in users are sent to log in. Non-admin users are rejected.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IConfiguration _config;
        private readonly IGenreRepository _genreRepo;

        /// <summary>
        /// Constructor for Admin controller, where we inject our needed repositories (Movie and Genre) and Config.
        /// </summary>
        /// <param name="movieRepo">Movie repository for movie related CRUD access to database.</param>
        /// <param name="genreRepo">Genre repository for genre related CRUD access to database.</param>
        /// <param name="config">Config to read needed information from appsettings.</param>
        public AdminController(IMovieRepository movieRepo, IGenreRepository genreRepo, IConfiguration config)
        {
            _movieRepo = movieRepo;
            _config = config;
            _genreRepo = genreRepo;
        }

        /// <summary>
        /// Index action method, which returns a view of the list of movies for the admin, with the ability to
        /// view, edit, and delete the movies. Also contains a link to Genre Administration.
        /// </summary>
        /// <returns>A View containing information for all movies in the database.</returns>
        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepo.ReadAllAsync();

            var model = movies.Select(movie =>
                new DisplayMovieVM
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Year = movie.Year,
                    LengthInMinutes = movie.LengthInMinutes,
                    Price = movie.Price.ToString("C"),
                    IMDB_URL = movie.IMDB_URL,
                    Genres = movie.GetGenres()

                });

            ViewData["Title"] = "Admin Movie List";
            return View(model);
        }

        /// <summary>
        /// CreateMovie Get action method, which has a form for user input for a new movie to insert into the database.
        /// </summary>
        /// <returns>A view to construct a movie to insert into the database.</returns>
        public IActionResult CreateMovie()
        {
            ViewData["Title"] = "Creating Movie";
            return View();
        }

        /// <summary>
        /// CreateMovie Post action method, used to validate our data then insert it into the database.
        /// </summary>
        /// <param name="movieVM">CreateMovieVM that we will construct our Movie from.</param>
        /// <returns>Once Movie is inserted we redirect to Admin Index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMovie(CreateMovieVM movieVM)
        {
            // Read the maximum year from the config file. 
            var maxYear = _config.GetValue<int>("MaxYear");
            
            // Error checking the movie Title.
            if (movieVM.Title == null)
            {
                ModelState.AddModelError("Title", "Movie must have a title.");
            }
            else if (movieVM.Title.Length > 50)
            {
                ModelState.AddModelError("Title", "Title must be less than 50 characters.");
            }

            // Error checking Year values.
            if (movieVM.Year < 1832 || movieVM.Year > maxYear)
            {
                ModelState.AddModelError("Year", $"The Movie Year must fall between 1832 and {maxYear}.");
            }

            // Error checking Length values.
            if (movieVM.LengthInMinutes < 0 || movieVM.LengthInMinutes > 1000)
            {
                ModelState.AddModelError("LengthInMinutes", "The Length of the movie must be between 0 and 1000 minutes.");
            }

            // Error checking Price values.

            Regex regex = new Regex(@"^[0-9]{0,3}(\.[0-9]{0,2}){0,1}$");
            if (!regex.IsMatch(movieVM.Price.ToString()))
            {
                ModelState.AddModelError("Price", "The Price must be less than 999.99, and a valid dollar amount.");
            }

            // Error checking IMDB URL.
            if (movieVM.IMDB_URL == null)
            {
                ModelState.AddModelError("IMDB_URL", "You must include the URL.");
            }

            if (ModelState.IsValid)
            {
                var movie = movieVM.GetMovieInstance();
                await _movieRepo.CreateAsyc(movie);
                return RedirectToAction("Index");
            }

            ViewData["Title"] = "Creating Movie";
            return View(movieVM);
        }

        /// <summary>
        /// EditMovie Get action method, which shows a view of the current information for the Movie and the ability to edit it.
        /// </summary>
        /// <param name="id">Id of movie to be edited.</param>
        /// <returns>A View containing the current information of the movie.</returns>
        public async Task<IActionResult> EditMovie(int id)
        {
            var movie = await _movieRepo.ReadAsync(id);

            // If the movie doesn't exist redirect to Admin Index.
            if (movie == null) 
            {
                return RedirectToAction("Index");
            }

            var model = new EditMovieVM
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Year = movie.Year,
                    LengthInMinutes = movie.LengthInMinutes,
                    Price = movie.Price,
                    IMDB_URL = movie.IMDB_URL,
                };

            ViewData["Title"] = "Editing Movie";
            return View(model);
        }

        /// <summary>
        /// EditMovie Post action method where we validate the data then update the database with the new movie information.
        /// </summary>
        /// <param name="movieVM">EditMovieVM containing the new information to write to the database.</param>
        /// <returns>Redirects to Admin Index after updating the database.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMovie(EditMovieVM movieVM)
        {
            var maxYear = _config.GetValue<int>("MaxYear");

            // Error checking Title values.
            if (movieVM.Title == null)
            {
                ModelState.AddModelError("Title", "Movie must have a title.");
            }
            else if (movieVM.Title.Length > 50)
            {
                ModelState.AddModelError("Title", "Title must be less than 50 characters.");
            }

            // Error checking Year values.
            if (movieVM.Year < 1832 || movieVM.Year > maxYear)
            {
                ModelState.AddModelError("Year", $"The Movie Year must fall between 1832 and {maxYear}.");
            }

            // Error checking Length value.
            if (movieVM.LengthInMinutes < 0 || movieVM.LengthInMinutes > 1000)
            {
                ModelState.AddModelError("LengthInMinutes", "The Length of the movie must be between 0 and 1000 minutes.");
            }

            // Error checking Price value.
            Regex regex = new Regex(@"^[0-9]{0,3}(\.[0-9]{0,2}){0,1}$");
            if (!regex.IsMatch(movieVM.Price.ToString()))
            {
                ModelState.AddModelError("Price", "The Price must be less than 999.99, and a valid dollar amount.");
            }

            // Error checking IMDB URL.
            if (movieVM.IMDB_URL == null)
            {
                ModelState.AddModelError("IMDB_URL", "You must include the URL.");
            }

            if (ModelState.IsValid)
            {
                var movie = movieVM.GetMovieInstance();
                await _movieRepo.UpdateAsyc(movie);
                return RedirectToAction("Index");
            }

            ViewData["Title"] = "Editing Movie";
            return View(movieVM);
        }

        /// <summary>
        /// DeleteMovie action method, which returns a view of for the user to confirm they wish to delete this movie.
        /// </summary>
        /// <param name="id">Id of movie to delete.</param>
        /// <returns>A View with for the user to confirm the deletion.</returns>
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _movieRepo.ReadAsync(id);

            if (movie == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["Title"] = "Deleting Movie";

            var model = new DeleteMovieVM
            {
                Id = movie.Id,
                Title = movie.Title
            };
            return View(model);
        }

        /// <summary>
        /// DeleteMovie action method to remove the movie from the database.
        /// </summary>
        /// <param name="id">Id of the movie to delete.</param>
        /// <returns>Redirects to Admin Index after deletion.</returns>
        [HttpPost, ActionName("DeleteMovie")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _movieRepo.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// MovieDetails action method, which shows information for the movie. Includes the ability to add or
        /// remove genres via links for each.
        /// </summary>
        /// <param name="id">Id of Movie to display.</param>
        /// <returns>A view with details about the selected movie.</returns>
        public async Task<IActionResult> MovieDetails(int id)
        {
            // Get the movie to display.
            var movie = await _movieRepo.ReadAsync(id);

            // If the movie doesn't exist, redirect to Admin Index.
            if (movie == null) 
            {
                return RedirectToAction("Index");
            }

            var model = new DisplayMovieVM
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                LengthInMinutes = movie.LengthInMinutes,
                Price = movie.Price.ToString("C"),
                IMDB_URL = movie.IMDB_URL,
                Genres = movie.GetGenres()
            };

            ViewData["Title"] = "Movie Details";

            return View(model);
        }

        /// <summary>
        /// AddGenre action method, which displays a view with the ability to add a genre to the movie.
        /// </summary>
        /// <param name="id">Id of movie to add a genre to.</param>
        /// <returns>A view with the ability to add a genre to the movie.</returns>
        public async Task<IActionResult> AddGenre(int id)
        {
            // Check that the movie exists, and genres exist. 
            var movie = await _movieRepo.ReadAsync(id);
            var genres = await _genreRepo.ReadAllAsync();

            if (movie == null || genres == null) 
            {
                return RedirectToAction("Index");
            }

            ViewData["Genres"] = genres;
            var model = new AddGenreVM
            {
                Id = movie.Id,
                Title = movie.Title,
                Genres = movie.GetGenres()
            };

            ViewData["Title"] = $"Adding Genre to {movie.Title}";
            return View(model);
        }

        /// <summary>
        /// AddGenre Post action method, where as long as the genre isn't null
        /// we add it to the movie. 
        /// </summary>
        /// <param name="genreVM">AddGenreVM containing movie and genre information.</param>
        /// <returns>After adding, we redirect to MovieDetails for this movie.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGenre(AddGenreVM genreVM)
        {
            var movieId = genreVM.Id;
            var genre = await _genreRepo.ReadAsync(genreVM.NewGenreId);

            if (genre == null)
            {
                return RedirectToAction("MovieDetails", new { id = movieId });
            }

            await _movieRepo.AddGenreAsync(movieId, genre);
            return RedirectToAction("MovieDetails", new { id = movieId });


        }

        /// <summary>
        /// RemoveGenre get action method, which returns a view with the ability to remove a genre from a movie.
        /// </summary>
        /// <param name="id">Id of movie to remove a genre from.</param>
        /// <returns>A view with the ability to remove a genre from the movie.</returns>
        public async Task<IActionResult> RemoveGenre(int id)
        {
            var movie = await _movieRepo.ReadAsync(id);

            if (movie == null)
            {
                return RedirectToAction("Index");
            }

            var genres = movie.MovieGenres.ToList();
            var model = new RemoveGenreVM
            {
                Id = movie.Id,
                Title = movie.Title,
                Genres = movie.GetGenres()
            };

            ViewData["Genres"] = genres;
            ViewData["Title"] = $"Removing Genre from {movie.Title}";
            return View(model);
        }
        /// <summary>
        /// RemoveGenre Post action method, where we remove the genre from the movie
        /// and redirect to the movie details.
        /// </summary>
        /// <param name="genreVM">RemoveGenreVM containing information of movie and genre.</param>
        /// <returns>After removing genre, we redirect to the MovieDetails for the movie.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveGenre(RemoveGenreVM genreVM)
        {
            var movieId = genreVM.Id;
            var genre = await _genreRepo.ReadAsync(genreVM.GenreIdToRemove);

            if( genre == null)
            {
                return RedirectToAction("MovieDetails", new { id = movieId });
            }

            await _movieRepo.RemoveGenreAsync(movieId, genre);
            return RedirectToAction("MovieDetails", new { id = movieId });
        }
    }
}
