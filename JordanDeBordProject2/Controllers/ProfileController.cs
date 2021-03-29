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
    /// <summary>
    /// Profile Controller, which handles client requests and directs them to the appropriate action method and then sends
    /// the response to user. It handles requests from clients to /profile/{action} where the action is the name of the method below.
    /// Non-logged in users are sent to log in.
    /// </summary>
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor for Profile Controller, where we inject our needed repositories into the Controller.
        /// </summary>
        /// <param name="profileRepository">Profile Repository for profile related database needs.</param>
        /// <param name="userRepository">User repository for user related database needs.</param>
        /// <param name="userManager">UserManager to manage database related to other User needs.</param>
        public ProfileController(IProfileRepository profileRepository, 
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager)
        {
            _profileRepository = profileRepository;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Index action method, users should not end up here, but redirect to home index if they somehow do.
        /// </summary>
        /// <returns>Redirects users to their Home Index.</returns>
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Create Get action method which displays a form for the user to provide information and create their
        /// profile.
        /// </summary>
        /// <returns>A view with a form for user to provide profile information to create.</returns>
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

        /// <summary>
        /// Create Post action method where we verify user's information is valid, then create the profile.
        /// </summary>
        /// <param name="profileVM">CreateProfileVM containing information for the profile to be created.</param>
        /// <returns>After creating, redirect to Home Index.</returns>
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

        /// <summary>
        /// Details get action method, used to display the user's profile information.
        /// </summary>
        /// <returns>A view containing profile information for the user.</returns>
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

        /// <summary>
        /// Edit get action method, used to display current information about the user's profile, and provides the ability to edit it.
        /// </summary>
        /// <returns>A view containing the user's current profile information and the ability to edit it.</returns>
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

        /// <summary>
        /// Edit post action method, where we validate the information and then update the User and Profile with the new information.
        /// </summary>
        /// <param name="profileVM">EditProfileVM containing new information for user and profile.</param>
        /// <returns>Redirects to Home index after updating profile and user.</returns>
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

        /// <summary>
        /// Delete get action method, which returns a view warning the user they are about to delete their profile.
        /// </summary>
        /// <returns>A view to verify user wants to delete their profile.</returns>
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

        /// <summary>
        /// Delete post action method, where the profile is deleted from the database.
        /// </summary>
        /// <param name="profileId">Id of profile to be deleted.</param>
        /// <returns>After deleting profile, redirect to Home Index.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int profileId) 
        {
            await _profileRepository.DeleteAsync(profileId);

            return RedirectToAction("Index", "Home");
        }
    }
}
