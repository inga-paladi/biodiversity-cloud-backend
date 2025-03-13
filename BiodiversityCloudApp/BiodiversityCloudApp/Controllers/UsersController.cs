using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BiodiversityCloudApp.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
// Create Create a User → POST /api/users
// Get All Users → GET /api/users
// Get User by ID → GET /api/users/{id}
// Update User → PUT /api/users/{id}
// Delete User → DELETE /api/users/{id}