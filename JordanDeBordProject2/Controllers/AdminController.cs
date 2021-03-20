using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateMovie()
        {
            return View();
        }

        public IActionResult EditMovie(int id)
        {
            return View();
        }

        public IActionResult DeleteMovie(int id)
        {
            return View();
        }
    }
}
