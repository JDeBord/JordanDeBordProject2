using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JordanDeBordProject2.Services;
using Microsoft.AspNetCore.Identity;
using JordanDeBordProject2.Models.Entities;

namespace JordanDeBordProject2.Controllers
{
    public class ProfileController : Controller
    {
        private IProfileRepository _profileRepository;
        private UserManager<ApplicationUser> _userManager;

        public ProfileController(IProfileRepository profileRepository, 
            UserManager<ApplicationUser> userManager)
        {
            _profileRepository = profileRepository;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            //// If user is not logged in, redirect to login.
            //if (User == null)
            //{
            //    return RedirectToAction("Account", "Identity", "LogIn");
            //}
            //// If user is an Admin, redirect to Admin index.
            //if (User.IsInRole("Admin")) 
            //{
            //    return RedirectToAction("Index", "Admin");
            //}
            //// If user already has a profile, redirect to Home index
            //var userId = await _userManager.GetUserIdAsync(User);
            //var user = await _
            //if ()
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            ViewData["Title"] = "Create A Profile";
            return View();
        }

        public async Task<IActionResult> Details()
        {
            //// If user is not logged in, redirect to login.
            //if (User == null)
            //{
            //    return RedirectToAction("Account", "Identity", "LogIn");
            //}
            //// If user is an Admin, redirect to Admin index.
            //if (User.IsInRole("Admin"))
            //{
            //    return RedirectToAction("Index", "Admin");
            //}
            //// If user already has a profile, redirect to Home index
            //var userId = _userManager.GetUserId(User);

            //if (!(await _profileRepository.CheckProfile(userId)))
            //{
            //    return RedirectToAction("Create");
            //}

            ViewData["Title"] = "Viewing Profile";
            return View();
        }

        public async Task<IActionResult> Edit()
        {
            //// If user is not logged in, redirect to login.
            //if (User == null)
            //{
            //    return RedirectToAction("Account", "Identity", "LogIn");
            //}
            //// If user is an Admin, redirect to Admin index.
            //if (User.IsInRole("Admin"))
            //{
            //    return RedirectToAction("Index", "Admin");
            //}
            //// If user already has a profile, redirect to Home index
            //var userId = _userManager.GetUserId(User);

            //if (!(await _profileRepository.CheckProfile(userId)))
            //{
            //    return RedirectToAction("Create");
            //}

            ViewData["Title"] = "Editing Profile";
            return View();
        }

        public async Task<IActionResult> Delete()
        {
            //// If user is not logged in, redirect to login.
            //if (User == null)
            //{
            //    return RedirectToAction("Account", "Identity", "LogIn");
            //}
            //// If user is an Admin, redirect to Admin index.
            //if (User.IsInRole("Admin"))
            //{
            //    return RedirectToAction("Index", "Admin");
            //}
            //// If user already has a profile, redirect to Home index
            //var userId = _userManager.GetUserId(User);

            //if (!(await _profileRepository.CheckProfile(userId)))
            //{
            //    return RedirectToAction("Create");
            //}

            ViewData["Title"] = "Deleting Profile";
            return View();
        }
    }
}
