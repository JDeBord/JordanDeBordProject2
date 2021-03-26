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
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

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
        
        public IActionResult Create() 
        {
            ViewData["Title"] = "Creating A Genre";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGenreVM genreVM) 
        {
            if (genreVM.GenreName == null)
            {
                ModelState.AddModelError("GenreName", "Genre must have a name.");
            }
            else if (genreVM.GenreName.Length > 20)
            {
                ModelState.AddModelError("GenreName", "Genre name must be 20 or fewer characters.");
            }

            if (ModelState.IsValid)
            {
                var genre = genreVM.GetGenreInstance();
                await _genreRepository.CreateAsyc(genre);
                return RedirectToAction("Index");
            }
            ViewData["Title"] = "Creating A Genre";
            return View(genreVM);
        }
        
        public async Task<IActionResult> Edit(int id)
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

            ViewData["Title"] = "Editing Genre";
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GenreVM genreVM) 
        {
            if (genreVM.GenreName == null)
            {
                ModelState.AddModelError("GenreName", "Genre must have a name.");
            }
            else if (genreVM.GenreName.Length > 20)
            {
                ModelState.AddModelError("GenreName", "Genre name must be 20 or fewer characters.");
            }

            if (ModelState.IsValid)
            {
                var genre = genreVM.GetGenreInstance();
                await _genreRepository.UpdateAsyc(genre);
                return RedirectToAction("Index");
            }
            ViewData["Title"] = "Editing Genre";
            return View(genreVM);
        }
        
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
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) 
        {
            await _genreRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        
        }
        
    }
}
