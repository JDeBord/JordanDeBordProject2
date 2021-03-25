using JordanDeBordProject2.Models;
using JordanDeBordProject2.Models.Entities;
using JordanDeBordProject2.Models.ViewModels;
using JordanDeBordProject2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProfileRepository _profileRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
            IProfileRepository profileRepository,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _profileRepository = profileRepository;
        }

        public async Task<IActionResult> Index()
        {
            // Admin users are sent to the Admin Index.
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
            var movies = await _profileRepository.GetPaidMoviesAsync(profile.Id);

            var model = movies.Select(movie =>
                new DisplayMovieHomeVM
                {
                    Id = movie.Movie.Id,
                    Title = movie.Movie.Title,
                    NumTimesWatched = movie.TimesWatched
                });
            
            var totalSpent = profile.TotalAmountSpent;
            var totalWatched = profile.TotalWatched;
            
            ViewData["Title"] = "Watched Movie List";
            ViewData["TotalWatched"] = totalWatched;
            ViewData["TotalSpent"] = totalSpent;
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult About() 
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
