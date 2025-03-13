using Microsoft.AspNetCore.Mvc;

namespace BiodiversityCloudApp.DTOs
{
    public class UserDto : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
