using Microsoft.AspNetCore.Mvc;

namespace BiodiversityCloudApp.Services
{
    public class IObservationService : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
