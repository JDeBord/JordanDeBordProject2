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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IConfiguration _config;
        private readonly IGenreRepository _genreRepo;
        public AdminController(IMovieRepository movieRepo, IGenreRepository genreRepo, IConfiguration config)
        {
            _movieRepo = movieRepo;
            _config = config;
            _genreRepo = genreRepo;
        }
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

        public IActionResult CreateMovie()
        {
            ViewData["Title"] = "Creating Movie";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMovie(CreateMovieVM movieVM)
        {
            var maxYear = _config.GetValue<int>("MaxYear");
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

        [HttpPost, ActionName("DeleteMovie")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _movieRepo.DeleteAsync(id);

            return RedirectToAction("Index");
        }

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
