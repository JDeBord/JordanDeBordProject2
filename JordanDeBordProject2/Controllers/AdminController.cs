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
        public AdminController(IMovieRepository movieRepo, IConfiguration config)
        {
            _movieRepo = movieRepo;
            _config = config;
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
            // Error checking Title values.
            if (movieVM.Title == null)
            {
                ModelState.AddModelError("Title", "Movie must have a title.");
            }
            else if (movieVM.Title.Length > 50)
            {
                ModelState.AddModelError("Title", "Title must be less than 50 characters.");
            }
            // Error checking Year values;
            if (movieVM.Year < 1832 || movieVM.Year > maxYear)
            {
                ModelState.AddModelError("Year", $"The Movie Year must fall between 1832 and {maxYear}.");
            }

            // Length


            // Price

            Regex regex = new Regex(@"^[0-9]{0,3}(\.[0-9]{0,2}){0,1}$");
            if (!regex.IsMatch(movieVM.Price.ToString()))
            {
                ModelState.AddModelError("Price", "The Price must be less than 999.99, and a valid dollar amount.");
            }

            // IMDB URL
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
            // Error checking Year values;
            if (movieVM.Year < 1832 || movieVM.Year > maxYear)
            {
                ModelState.AddModelError("Year", $"The Movie Year must fall between 1832 and {maxYear}.");
            }

            // Length


            // Price

            Regex regex = new Regex(@"^[0-9]{0,3}(\.[0-9]{0,2}){0,1}$");
            if (!regex.IsMatch(movieVM.Price.ToString()))
            {
                ModelState.AddModelError("Price", "The Price must be less than 999.99, and a valid dollar amount.");
            }

            // IMDB URL
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
            ViewData["Title"] = "Deleting Movie";
            return View();
        }

        public async Task<IActionResult> MovieDetails(int id)
        {
            var movie = await _movieRepo.ReadAsync(id);

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
            return View();
        }
        public async Task<IActionResult> RemoveGenre(int id)
        {
            return View();
        }
    }
}
