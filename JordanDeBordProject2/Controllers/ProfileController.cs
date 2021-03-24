using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JordanDeBordProject2.Services;
using Microsoft.AspNetCore.Identity;
using JordanDeBordProject2.Models.Entities;
using JordanDeBordProject2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace JordanDeBordProject2.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private IProfileRepository _profileRepository;
        private IUserRepository _userRepository;
        private UserManager<ApplicationUser> _userManager;


        public ProfileController(IProfileRepository profileRepository, 
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager)
        {
            _profileRepository = profileRepository;
            _userManager = userManager;
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Create()
        {

            // If user is an Admin, redirect to Admin index.
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            // If user already has a profile, redirect to Home index
            var userName = User.Identity.Name;
            var user = await _userRepository.ReadAsync(userName);

            if (await _profileRepository.CheckProfile(user.Id))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["UserId"] = user.Id;
            ViewData["Title"] = "Create A Profile";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProfileVM profileVM)
        {
            // Error Checking

            if (ModelState.IsValid)
            {
                var profile = profileVM.GetProfileInstance();
                await _profileRepository.CreateAsyc(profileVM.ApplicationUserId, profile);
                return RedirectToAction("Index", "Home");
            }

            ViewData["Title"] = "Create A Profile";
            return View();
        }

        public async Task<IActionResult> Details()
        {
            // If user is an Admin, redirect to Admin index.
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            // If user doesn't have a profile, redirect to create one.
            var userId = _userManager.GetUserId(User);

            if (!(await _profileRepository.CheckProfile(userId)))
            {
                return RedirectToAction("Create");
            }

            ViewData["Title"] = "Viewing Your Profile";
            
            var profile = await _profileRepository.ReadByUserAsync(userId);

            var user = await _userRepository.ReadByIdAsync(userId);

            var model = new DisplayProfileVM
                {
                    Name = $"{user.LastName}, {user.FirstName}",
                    CCNum = $"********{profile.CCNum[8..]}",
                    CCExp = profile.CCExp,
                    Address = profile.GetFormattedAddress(),
                    AmountSpent = profile.TotalAmountSpent.ToString("c")
                };

            return View(model);
        }

        public async Task<IActionResult> Edit()
        {
            // If user is an Admin, redirect to Admin index.
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            // If user doesn't have a profile, redirect to create one.
            var userId = _userManager.GetUserId(User);

            if (!(await _profileRepository.CheckProfile(userId)))
            {
                return RedirectToAction("Create");
            }

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
