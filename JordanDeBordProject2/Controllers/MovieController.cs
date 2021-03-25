using JordanDeBordProject2.Models.Entities;
using JordanDeBordProject2.Models.ViewModels;
using JordanDeBordProject2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMovieRepository _movieRepository;

        public MovieController (IProfileRepository profileRepository,
            IUserRepository userRepository,
            IMovieRepository movieRepository,
            UserManager<ApplicationUser> userManager)
        {
            _profileRepository = profileRepository;
            _userManager = userManager;
            _userRepository = userRepository;
            _movieRepository = movieRepository;
        }

        public async Task<IActionResult> Index()
        {
            // redirect admin
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            // If user doesn't have a profile, redirect to create one.
            var userId = _userManager.GetUserId(User);

            if (!(await _profileRepository.CheckProfile(userId)))
            {
                return RedirectToAction("Create", "Profile");
            }
            var profile = await _profileRepository.ReadByUserAsync(userId);

            var movies = await _movieRepository.ReadAllAsync();

            var boughtMovies = await _profileRepository.GetPaidMoviesAsync(profile.Id);

            var model = movies.Select(movie =>
                 new DisplayMovieIndexVM
                {
                     Id = movie.Id,
                     Title = movie.Title,
                     WatchStatus = "Not Watched"
                });
            // Update the watched status for paid for and watched movies.
            foreach (var bm in boughtMovies)
            {
                if (bm.TimesWatched > 0)
                {
                    var updatedMovie = model.FirstOrDefault(movie => movie.Id == bm.MovieId);
                    if (updatedMovie != null)
                    {
                        updatedMovie.WatchStatus = "Watched";
                    }
                }
            }

            // Count of all movies.
            ViewData["TotalMovies"] = movies.Count;
            
            ViewData["Title"] = "All Movies";
            
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            // Redirect Admin
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            // If user doesn't have a profile, redirect to create one.
            var userId = _userManager.GetUserId(User);

            if (!(await _profileRepository.CheckProfile(userId)))
            {
                return RedirectToAction("Create", "Profile");
            }

            // Construct view model (id, title, year, length, price)
            var movie = await _movieRepository.ReadAsync(id);

            if (movie == null)
            {
                return RedirectToAction("Index");
            }
            var model = new DetailsMovieVM
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                LengthInMin = movie.LengthInMinutes,
                Price = movie.Price
            };

            ViewData["Title"] = "Movie Details";
            return View(model);
        }

        public async Task<IActionResult> Pay(int id)
        {
            // Redirect Admin
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            // If user doesn't have a profile, redirect to create one.
            var userId = _userManager.GetUserId(User);

            if (!(await _profileRepository.CheckProfile(userId)))
            {
                return RedirectToAction("Create", "Profile");
            }

            // Redirect if already paid 

            // Construct view model (id, title, price)

            ViewData["Title"] = "Paying for Movie";
            return View();
        }

        [HttpPost, ActionName("Pay")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayConfirmed(int id)
        {
            // charge card
            // create paid movie
            // redirect to movie/watch/id
            return View();
        }

        public async Task<IActionResult> Watch(int id)
        {
            // Redirect Admin
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            // If user doesn't have a profile, redirect to create one.
            var userId = _userManager.GetUserId(User);

            if (!(await _profileRepository.CheckProfile(userId)))
            {
                return RedirectToAction("Create", "Profile");
            }

            // Check if paid for, if not redirect to pay/id

            // If paid for, update # times watched

            // display imdb url

            ViewData["Title"] = "Watching Movie";
            return View();
        }
    }
}
