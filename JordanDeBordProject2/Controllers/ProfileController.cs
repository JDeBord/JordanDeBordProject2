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
using System.Text.RegularExpressions;
using JordanDeBordProject2.Models;

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
            var userId = _userManager.GetUserId(User);
            var profile = await _profileRepository.ReadByUserAsync(userId);

            if (profile != null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["UserId"] = userId;
            ViewData["Title"] = "Create Your Profile";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProfileVM profileVM)
        {
            // Error Checking CC number.
            Regex regex = new Regex(@"^[0-9]{12}$");
            if (profileVM.CCNum == null)
            {
                ModelState.AddModelError("CCNum", "The Credit Card Number must be exactly 12 numeric digits.");
            }
            else if (!regex.IsMatch(profileVM.CCNum))
            {
                ModelState.AddModelError("CCNum", "The Credit Card Number must be exactly 12 numeric digits.");
            }

            // Error Checking Address Line 1.
            if (profileVM.AddLine1 == null)
            {
                ModelState.AddModelError("AddLine1", "Address Line 1 is required.");
            }
            else if (profileVM.AddLine1.Length > 100 )
            {
                ModelState.AddModelError("AddLine1", "The Address Must be 100 or fewer characters long.");
            }
            // Error Checking Address Line 2.
            if (profileVM.AddLine2 != null && profileVM.AddLine2.Length > 30)
            {
                ModelState.AddModelError("AddLine2", "Address Line 2 must be 30 or fewer characters long.");
            }
            // Error Checking City.
            if (profileVM.City == null)
            {
                ModelState.AddModelError("City", "City is required.");
            }
            else if (profileVM.City.Length > 50)
            {
                ModelState.AddModelError("City", "City can not be more than 50 characters long.");
            }

            // Error Checking State.
            if (profileVM.State == null)
            {
                ModelState.AddModelError("State", "State must be the two character state code.");
            }
            else if (profileVM.State.Length != 2)
            {
                ModelState.AddModelError("State", "State must be the two character state code.");
            }
            else if (!Enum.IsDefined(typeof(StateCode), profileVM.State.ToUpper()))
            {
                ModelState.AddModelError("State", "You must use a valid 2 digit state code.");
            }
            // Error Checking ZIP.
            regex = new Regex(@"^[0-9]{5}$");
            if (profileVM.ZIPCode == null) 
            {
                ModelState.AddModelError("ZIPCode", "The ZIP Code must be 5 numeric digits long.");
            }
            else if (!regex.IsMatch(profileVM.ZIPCode))
            {
                ModelState.AddModelError("ZIPCode", "The ZIP Code must be 5 numeric digits long.");
            }

            if (ModelState.IsValid)
            {
                var profile = profileVM.GetProfileInstance();
                await _profileRepository.CreateAsyc(profileVM.ApplicationUserId, profile);
                return RedirectToAction("Index", "Home");
            }

            // we need to store the UserID back in the ViewData
            var userId = _userManager.GetUserId(User);
            ViewData["UserId"] = userId;
            ViewData["Title"] = "Create Your Profile";
            return View(profileVM);
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
            var profile = await _profileRepository.ReadByUserAsync(userId);
            var userName = User.Identity.Name;

            if (profile == null)
            {
                return RedirectToAction("Create");
            }

            ViewData["Title"] = "Viewing Your Profile";

            var user = await _userRepository.ReadAsync(userName);

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
            var userName = User.Identity.Name;
            var profile = await _profileRepository.ReadByUserAsync(userId);

            if (profile == null)
            {
                return RedirectToAction("Create");
            }
            var user = await _userRepository.ReadAsync(userName);
            var model = new EditProfileVM
            {
                ProfileId = profile.Id,
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CCNum = profile.CCNum,
                CCExp = profile.CCExp,
                AddLine1 = profile.AddLine1,
                AddLine2 = profile.AddLine2,
                City = profile.City,
                State = profile.State,
                ZIPCode = profile.ZIPCode
            };

            ViewData["Title"] = "Editing Your Profile";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileVM profileVM) 
        {
            // Error checking first name.
            if (profileVM.FirstName == null)
            {
                ModelState.AddModelError("FirstName", "First Name is required.");
            }
            else if (profileVM.FirstName.Length > 50)
            {
                ModelState.AddModelError("FirstName", "First Name must be 50 or fewer characters.");
            }
            // Error checking last name.
            if (profileVM.LastName == null)
            {
                ModelState.AddModelError("LastName", "Last Name is required.");
            }
            else if (profileVM.LastName.Length > 50)
            {
                ModelState.AddModelError("LastName", "Last Name must be 50 or fewer characters.");
            }

            // Error Checking CC number.
            Regex regex = new Regex(@"^[0-9]{12}$");
            if (profileVM.CCNum == null)
            {
                ModelState.AddModelError("CCNum", "The Credit Card Number must be exactly 12 numeric digits.");
            }
            else if (!regex.IsMatch(profileVM.CCNum))
            {
                ModelState.AddModelError("CCNum", "The Credit Card Number must be exactly 12 numeric digits.");
            }

            // Error Checking Address Line 1.
            if (profileVM.AddLine1 == null)
            {
                ModelState.AddModelError("AddLine1", "Address Line 1 is required.");
            }
            else if (profileVM.AddLine1.Length > 100)
            {
                ModelState.AddModelError("AddLine1", "The Address Must be 100 or fewer characters long.");
            }
            // Error Checking Address Line 2.
            if (profileVM.AddLine2 != null && profileVM.AddLine2.Length > 30)
            {
                ModelState.AddModelError("AddLine2", "Address Line 2 must be 30 or fewer characters long.");
            }
            // Error Checking City.
            if (profileVM.City == null)
            {
                ModelState.AddModelError("City", "City is required.");
            }
            else if (profileVM.City.Length > 50)
            {
                ModelState.AddModelError("City", "City can not be more than 50 characters long.");
            }

            // Error Checking State.
            if (profileVM.State == null)
            {
                ModelState.AddModelError("State", "State must be the two character state code.");
            }
            else if (profileVM.State.Length != 2)
            {
                ModelState.AddModelError("State", "State must be the two character state code.");
            }
            else if (!Enum.IsDefined(typeof(StateCode), profileVM.State.ToUpper()))
            {
                ModelState.AddModelError("State", "You must use a valid 2 digit state code.");
            }
            // Error Checking ZIP.
            regex = new Regex(@"^[0-9]{5}$");
            if (profileVM.ZIPCode == null)
            {
                ModelState.AddModelError("ZIPCode", "The ZIP Code must be 5 numeric digits long.");
            }
            else if (!regex.IsMatch(profileVM.ZIPCode))
            {
                ModelState.AddModelError("ZIPCode", "The ZIP Code must be 5 numeric digits long.");
            }

            if (ModelState.IsValid)
            {
                var profile = profileVM.GetProfileInstance();
                var user = profileVM.GetUserInstance();

                await _profileRepository.UpdateAsyc(profile);
                await _userRepository.UpdateAsync(user);

                return RedirectToAction("Index", "Home");
            }

            ViewData["Title"] = "Editing Your Profile";
            return View(profileVM);
        }

        public async Task<IActionResult> Delete()
        {
            // If user is an Admin, redirect to Admin index.
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            // If user doesn't have a profile, redirect to create one.
            var userId = _userManager.GetUserId(User);
            var profile = await _profileRepository.ReadByUserAsync(userId);

            if (profile == null)
            {
                return RedirectToAction("Create");
            }
            
            var model = new DeleteProfileVM
            {
                Id = profile.Id
            };

            ViewData["Title"] = "Deleting Profile";

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int profileId) 
        {
            await _profileRepository.DeleteAsync(profileId);

            return RedirectToAction("Index", "Home");
        }
    }
}
