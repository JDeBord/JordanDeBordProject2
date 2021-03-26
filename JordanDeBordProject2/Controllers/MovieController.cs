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
            // Redirect admins to admin Index.
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

            // Get the user's profile, all the movies, and all the user's purchased movies. 

            var profile = await _profileRepository.ReadByUserAsync(userId);

            var movies = await _movieRepository.ReadAllAsync();

            var boughtMovies = await _profileRepository.GetPaidMoviesAsync(profile.Id);
            // Select a view model for each movie, and turn it into a list so that I can adjust the values,
            //      as an IEnumerable wouldn't allow me to as easily.

            var model = movies.Select(movie =>
                 new DisplayMovieIndexVM
                {
                     Id = movie.Id,
                     Title = movie.Title,
                     WatchStatus = "Not Watched"
                }).ToList();

            // Update the watched status for paid for and watched movies.
            foreach (var bm in boughtMovies)
            {
                if (bm.TimesWatched > 0)
                {
                    model.FirstOrDefault(movie => movie.Id == bm.MovieId).WatchStatus = "Watched";
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

        public async Task<IActionResult> Pay([Bind(Prefix ="id")]int movieId)
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

            // If Movie doesn't exist, redirect to movie index.
            var movie = await _movieRepository.ReadAsync(movieId);
            if (movie == null)
            {
                return RedirectToAction("Index");
            }

            // Check if paid for, if not redirect to pay/id
            var profile = await _profileRepository.ReadByUserAsync(userId);
            var paidMovies = await _profileRepository.GetPaidMoviesAsync(profile.Id);
            var paidFor = false;
            foreach (var mov in paidMovies)
            {
                if (mov.MovieId == movieId)
                {
                    paidFor = true;
                    break;
                }
            }

            if (paidFor)
            {
                return RedirectToAction("Watch", "Movie", new { id = movieId });
            }
            var model = new PayMovieVM
            {
                MovieId = movie.Id,
                ProfileId = profile.Id,
                Title = movie.Title,
                Price = movie.Price.ToString("C"),
            };

            ViewData["Title"] = $"Paying for {movie.Title}";
            return View(model);
        }

        [HttpPost, ActionName("Pay")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayConfirmed(PayMovieVM movieVM)
        {
            // This is where I would attempt to charge the card.
            // If the Charge failed, I would return View(movieVM) and notify customer their card was declined.

            // create paid movie
            var movie = await _movieRepository.ReadAsync(movieVM.MovieId);

            await _profileRepository.AddPaidMovie(movieVM.ProfileId, movie);
            // redirect to movie/watch/id
            return RedirectToAction("Watch", "Movie", new { id = movie.Id });
        }

        public async Task<IActionResult> Watch([Bind(Prefix = "id")] int movieId)
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

            var movie = await _movieRepository.ReadAsync(movieId);

            // If the movie doesn't exist, redirect to Movie Index
            if (movie == null)
            {
                return RedirectToAction("Index");
            }

            // Check if paid for, if not redirect to pay/id
            var profile = await _profileRepository.ReadByUserAsync(userId);
            var paidMovies = await _profileRepository.GetPaidMoviesAsync(profile.Id);
            var paidFor = false;
            foreach (var mov in paidMovies)
            {
                if (mov.MovieId == movieId)
                {
                    paidFor = true;
                    break;
                }
            }

            if (!paidFor)
            {
                return RedirectToAction("Pay", "Movie", new { id = movieId });
            }
            // If paid for, update # times watched.
            await _profileRepository.UpdateWatchedCountAsync(profile.Id, movieId);

            var model = new WatchMovieVM
            { 
                Id = movie.Id,
                Title = movie.Title,
                IMDB_URL = movie.IMDB_URL
            };
            
            ViewData["Title"] = $"Watching {movie.Title}";
            return View(model);
        }
    }
}
