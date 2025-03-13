using Microsoft.AspNetCore.Mvc;

namespace BiodiversityCloudApp.Services
{
    public class IUserService : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
