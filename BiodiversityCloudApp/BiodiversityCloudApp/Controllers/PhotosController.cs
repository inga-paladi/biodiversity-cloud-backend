using Microsoft.AspNetCore.Mvc;

namespace BiodiversityCloudApp.Controllers
{
    public class PhotosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}


/*
 Upload a Photo → POST /api/photos
Get All Photos → GET /api/photos
Get Photo by ID → GET /api/photos/{id}
Delete Photo → DELETE /api/photos/{id}
 */