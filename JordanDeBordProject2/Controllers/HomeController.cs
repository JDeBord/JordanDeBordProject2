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
    /// <summary>
    /// Home Controller, which handles client requests and directs them to the appropriate action method and then sends
    /// the response to user. It handles requests from clients to /home/{action} where the action is the name of the method below.
    /// Non-logged in users are sent to log in.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProfileRepository _profileRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor for the Home Controller, which injects our Profile Repository, Usermanager,
        /// and default Logger into the Controller.
        /// </summary>
        /// <param name="logger">Default logger in Home Controller.</param>
        /// <param name="profileRepository">Profile Repository used to interact with the database.</param>
        /// <param name="userManager">UserManager used to interact with database.</param>
        public HomeController(ILogger<HomeController> logger,
            IProfileRepository profileRepository,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _profileRepository = profileRepository;
        }

        /// <summary>
        /// Index action method, which returns the default landing page for signed in non-admin users.
        /// </summary>
        /// <returns>A view containing a list of watched movies for the profile.</returns>
        public async Task<IActionResult> Index()
        {
            // Admin users are sent to the Admin Index.
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            // If user doesn't have a profile, redirect to create one.
            var userId = _userManager.GetUserId(User);
            var profile = await _profileRepository.ReadByUserAsync(userId);

            if (profile == null)
            {
                return RedirectToAction("Create", "Profile");
            }

            // Get all the paid movies for the profile of the current user.
            var movies = await _profileRepository.GetPaidMoviesAsync(profile.Id);

            // Select a View Model for each movie. 
            var model = movies.Select(movie =>
                new DisplayMovieHomeVM
                {
                    Id = movie.Movie.Id,
                    Title = movie.Movie.Title,
                    NumTimesWatched = movie.TimesWatched
                });
            
            var totalSpent = profile.TotalAmountSpent;
            var totalWatched = profile.TotalWatched;
            var totalMovies = movies.Count;
            
            ViewData["Title"] = "Watched Movie List";
            ViewData["TotalWatched"] = totalWatched;
            ViewData["TotalSpent"] = totalSpent;
            ViewData["TotalMovies"] = totalMovies;

            return View(model);
        }

        /// <summary>
        /// About action method, which returns the default "About" page for our application. 
        /// We allow non-logged in users to access this page.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult About() 
        {
            return View();
        }

        /// <summary>
        /// Default Error action method.
        /// </summary>
        /// <returns>Default Error View.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
