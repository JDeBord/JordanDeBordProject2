using JordanDeBordProject2.Models.ViewModels;
using JordanDeBordProject2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Controllers
{
    /// <summary>
    /// Genre Controller, which handles client requests and directs them to the appropriate action method and then sends
    /// the response to user. It handles requests from clients to /genre/{action} where the action is the name of the method below.
    /// Non-logged in users are sent to log in. Non-Admin users are rejected.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        /// <summary>
        /// Constructor for Genre Controller, where we inject our Genre Repository into the Controller.
        /// </summary>
        /// <param name="genreRepository">Genre repository through which we interact with our database.</param>
        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        /// <summary>
        /// Genre action method, which will get a list of all genres and display it for the admin.
        /// The admin is then able to edit or delete each one.
        /// </summary>
        /// <returns>A View displaying the list of genres for the admin.</returns>
        public async Task<IActionResult> Index()
        {
            var genres = await _genreRepository.ReadAllAsync();

            var model = genres.Select(genre =>
                new GenreVM 
                {
                    Id = genre.Id,
                    GenreName = genre.Name
                });

            ViewData["Title"] = "Admin Genre List";
            return View(model);
        }

        /// <summary>
        /// Create Get action method, which has a form for the admin to input a new genre to insert into the database.
        /// </summary>
        /// <returns>A View to construct an orchestra and insert it into our database.</returns>
        public IActionResult Create() 
        {
            ViewData["Title"] = "Creating A Genre";
            return View();
        }

        /// <summary>
        /// Create Post action method, used to validate our data then insert it into the database.
        /// </summary>
        /// <param name="genreVM">CreateGenreVM that we will construct our Genre from.</param>
        /// <returns>Once genre is inserted, redirect to Genre Index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGenreVM genreVM) 
        {
            // Data validation for Genre Name. Must be between 1 and 20 characters.
            if (genreVM.GenreName == null)
            {
                ModelState.AddModelError("GenreName", "Genre must have a name.");
            }
            else if (genreVM.GenreName.Length > 20)
            {
                ModelState.AddModelError("GenreName", "Genre name must be 20 or fewer characters.");
            }

            // If the Model has no errors, create the genre and insert it into the database. Redirect to Index.
            if (ModelState.IsValid)
            {
                var genre = genreVM.GetGenreInstance();
                await _genreRepository.CreateAsyc(genre);
                return RedirectToAction("Index");
            }

            ViewData["Title"] = "Creating A Genre";
            return View(genreVM);
        }

        /// <summary>
        /// Edit Get action method. Loads a form with the current Genre information, and gives the ability to edit the name.
        /// </summary>
        /// <param name="id">Id of the genre to update.</param>
        /// <returns>View containing form to update name of the genre.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var genre = await _genreRepository.ReadAsync(id);
            
            // If the genre doesn't exist, redirect to Genre Index.
            if (genre == null)
            {
                return RedirectToAction("Index");
            }

            var model = new GenreVM
            {
                Id = genre.Id,
                GenreName = genre.Name
            };

            ViewData["Title"] = "Editing Genre";
            return View(model);
        }

        /// <summary>
        /// Edit Post action method. Used to validate our data then update the genre in the database. 
        /// </summary>
        /// <param name="genreVM">GenreVM containing genre information to update.</param>
        /// <returns>Once the genre has been updated, redirect to Genre Index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GenreVM genreVM) 
        {
            // Validate the Genre name is between 1-20 characters.
            if (genreVM.GenreName == null)
            {
                ModelState.AddModelError("GenreName", "Genre must have a name.");
            }
            else if (genreVM.GenreName.Length > 20)
            {
                ModelState.AddModelError("GenreName", "Genre name must be 20 or fewer characters.");
            }

            // If the ModelState is valid, update the genre in the database. 
            if (ModelState.IsValid)
            {
                var genre = genreVM.GetGenreInstance();
                await _genreRepository.UpdateAsyc(genre);
                return RedirectToAction("Index");
            }

            ViewData["Title"] = "Editing Genre";
            return View(genreVM);
        }

        /// <summary>
        /// Delete Get action method, which verifies the user wants to delete the genre.
        /// </summary>
        /// <param name="id">Id of genre to be deleted.</param>
        /// <returns>A View with a confirmation that the user wishes to delete the genre.</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _genreRepository.ReadAsync(id);

            if (genre == null)
            {
                return RedirectToAction("Index");
            }

            var model = new GenreVM
            { 
                Id = genre.Id,
                GenreName = genre.Name
            };
            
            ViewData["Title"] = "Deleting Genre";
            return View(model);
        }

        /// <summary>
        /// Delete Post action method, which removes the genre from the database and redirects to Index.
        /// </summary>
        /// <param name="id">Id of Genre to remove from database.</param>
        /// <returns>Redirects to Index after removing genre.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) 
        {
            await _genreRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        
        }
        
    }
}
