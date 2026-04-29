using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Pdnink.Controllers
{
    [Authorize]
    //[RedirectingAction]
    [AutoValidateAntiforgeryToken]
    public class BaseController : Controller
    {
        public string UserName { get => GetClaimUserName(); }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private string GetClaimUserName()
        {
            
            string UserName = string.Empty;
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                UserName = identity.FindFirst("username").Value;
            }
            return UserName;
        }

    }
}
