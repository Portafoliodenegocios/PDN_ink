using Microsoft.AspNetCore.Mvc;
using Pdnink.Controllers;
using Pdnink_Coremvc.Helpers;
using Pdnink_Coremvc.Models;
using System.Diagnostics;

namespace Pdnink_Coremvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Check(string Username)
        {
           var decodedUserName = Username.FromBase64().ToLower();
            //Check if the username in Claims is the same of the requested login. 
            if (decodedUserName != (UserName ?? "").ToLower())
            {
                return RedirectToAction("Logout", "Account");
            }

            return RedirectToAction("Index");

        }

        public IActionResult Index()
        {
          
            string token_= Request.Cookies["PdnInkToken"];

           ViewData["TokenRH"] = token_;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult originacion()
        {

            string token_ = Request.Cookies["PdnInkToken"];

            ViewData["TokenRH"] = token_;
            return View();
        }




    }
}
