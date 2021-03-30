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
    /// <summary>
    /// Movie Controller, which handles client requests and directs them to the appropriate action method and then sends
    /// the response to user. It handles requests from clients to /movie/{action} where the action is the name of the method below.
    /// Non-logged in users are sent to log in.
    /// </summary>
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IProfileRepository _profileRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;

        /// <summary>
        /// Constructor for Movie Controller, where we inject our needed repositories and services.
        /// </summary>
        /// <param name="profileRepository">Profile repository used to access database for profile CRUD operations.</param>
        /// <param name="movieRepository">Movie repository used to access database for movie CRUD operations.</param>
        /// <param name="userManager">UserManager to provide needed user interactions with Database.</param>
        /// <param name="genreRepository">Genre repository used to access database for genre CRUD operations.</param>
        public MovieController (IProfileRepository profileRepository,
            IMovieRepository movieRepository,
            UserManager<ApplicationUser> userManager,
            IGenreRepository genreRepository)
        {
            _profileRepository = profileRepository;
            _userManager = userManager;
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
        }

        /// <summary>
        /// Index action method, which returns a view of all the movies in the database for users to browse.
        /// </summary>
        /// <returns>A View with a list of movies the the ability to view details about them.</returns>
        public async Task<IActionResult> Index()
        {
            // Redirect admins to admin Index.
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

            // Get all the movies and all the user's purchased movies. 
            var movies = await _movieRepository.ReadAllAsync();

            var boughtMovies = await _profileRepository.GetPaidMoviesAsync(profile.Id);
            // Select a view model for each movie, and turn it into a list so that we can adjust the values.

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

        /// <summary>
        /// Details action method, which responds with a view containing details about the movie.
        /// </summary>
        /// <param name="id">Id of movie to display details about.</param>
        /// <returns>A View with details of the movie.</returns>
        public async Task<IActionResult> Details(int id)
        {
            // Redirect Admin to Admin Index.
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

            // Construct view model of the movie. If the movie doesn't exist redirect to Index.
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
                Price = movie.Price,
                Genres = movie.GetGenres()
            };

            ViewData["Title"] = "Movie Details";
            return View(model);
        }

        /// <summary>
        /// Pay Get action method, which loads a view which displays a message about paying for a movie.
        /// </summary>
        /// <param name="movieId">Id of the Movie for the customer to purchase.</param>
        /// <returns>View with information about the possible purchase, for the user to confirm.</returns>
        public async Task<IActionResult> Pay([Bind(Prefix ="id")]int movieId)
        {
            // Redirect Admin to Admin Index.
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

            // If Movie doesn't exist, redirect to movie index.
            var movie = await _movieRepository.ReadAsync(movieId);
            if (movie == null)
            {
                return RedirectToAction("Index");
            }

            // Check if user has already paid for movie.
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

            // If they have already paid for it, redirect to /movie/watch/id
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

        /// <summary>
        /// Pay Post action method, which is where customer is charged for the movie, the PaidMovie is created,
        /// and we redirect the user to watch their new movie.
        /// </summary>
        /// <param name="movieVM">ViewModel containing information about the movie to be purchased and user.</param>
        /// <returns>Redirects user to watch the movie after charging them.</returns>
        [HttpPost, ActionName("Pay")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayConfirmed(PayMovieVM movieVM)
        {
            // This is where I would attempt to charge the card.
            // If the Charge failed, I would return View(movieVM) and notify customer their card was declined.

            // Create the paid movie.
            var movie = await _movieRepository.ReadAsync(movieVM.MovieId);
            await _profileRepository.AddPaidMovie(movieVM.ProfileId, movie);
            
            // redirect to movie/watch/id
            return RedirectToAction("Watch", "Movie", new { id = movie.Id });
        }

        /// <summary>
        /// Watch action method which returns a view containing the link to the IMDB profile of the movie
        /// to simulate watching the movie. 
        /// </summary>
        /// <param name="movieId">Id of movie to be provided to user.</param>
        /// <returns>View containing link to IMDB profile of the movie.</returns>
        public async Task<IActionResult> Watch([Bind(Prefix = "id")] int movieId)
        {
            // Redirect Admin to Admin Index.
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

            var movie = await _movieRepository.ReadAsync(movieId);

            // If the movie doesn't exist, redirect to Movie Index
            if (movie == null)
            {
                return RedirectToAction("Index");
            }

            // Check if the user has paid for the movie. If not redirect to pay/id.
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

            // If paid for, update the number of times watched.
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

        /// <summary>
        /// BrowseByGenre action method, which returns a list of the movies with that genre
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> BrowseByGenre(int id) 
        {
            // Redirect admins to admin Index.
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

        
            var boughtMovies = await _profileRepository.GetPaidMoviesAsync(profile.Id);

            // If Genre doesn't exist, redirect to Browse.
            var genre = await _genreRepository.ReadAsync(id);

            if (genre == null)
            {
                return RedirectToAction("Browse");
            }
            var moviesWithGenre = await _movieRepository.ReadAllMovieGenreAsync(id);

            var model = moviesWithGenre.Select(m =>
                 new DisplayMovieIndexVM
                 {
                     Id = m.MovieId,
                     Title = m.Movie.Title,
                     WatchStatus = "Not Watched"
                 }).ToList();
            
            // For each film the user owns, see if it matches one from our search. If so set it to Watched if it has been.
            foreach (var bm in boughtMovies)
            {
                if (bm.TimesWatched > 0)
                {
                    var matchedMovie = model.FirstOrDefault(movie => movie.Id == bm.MovieId);

                    if (matchedMovie != null)
                    {
                        matchedMovie.WatchStatus = "Watched";
                    }
                }
            }
            

            // Count of all movies.
            ViewData["TotalMovies"] = moviesWithGenre.Count;

            ViewData["Title"] = $"All {genre.Name} Movies";

            return View(model);

        }

        /// <summary>
        /// Browse action method which returns a view with a list of all Genres that the user can then browse by.
        /// </summary>
        /// <returns>A view containing options for user to choose genre to browse by.</returns>
        public async Task<IActionResult> Browse()
        {
            // Redirect admins to admin Index.
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

            var genres = await _genreRepository.ReadAllAsync();

            var model = genres.Select(g =>
                new GenreVM
                {
                    Id = g.Id,
                    GenreName = g.Name
                });

            ViewData["Title"] = "Browsing All Genres";
            ViewData["GenreCount"] = genres.Count;

            return View(model);
        }
    }
}
