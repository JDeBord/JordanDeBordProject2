using JordanDeBordProject2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.ViewComponents
{
    public class AppNameViewComponent : ViewComponent
    {
        private readonly IConfiguration _config;

        public AppNameViewComponent(IConfiguration config) 
        {
            _config = config;
        }

        public IViewComponentResult Invoke() 
        {
            var appName = _config.GetValue<string>("ApplicationName");
            var model = new AppNameVM { AppName = appName };
            return View(model);
        }

    }
}
