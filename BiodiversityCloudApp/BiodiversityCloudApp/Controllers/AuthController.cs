using Microsoft.AspNetCore.Mvc;

namespace BiodiversityCloudApp.Controllers
{

    public class AuthController : Controller
    {

        
        public IActionResult Index()
        {
            return View();
        }
    }
}

/*
Register a User → POST /api/auth/register
Login → POST /api/auth/login
Get Current User → GET /api/auth/me
Logout → POST /api/auth/logout
 */