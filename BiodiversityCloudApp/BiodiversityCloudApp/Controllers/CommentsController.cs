using Microsoft.AspNetCore.Mvc;

namespace BiodiversityCloudApp.Controllers
{
    public class CommentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

/*
 Create a Comment → POST /api/comments
Get All Comments → GET /api/comments
Get Comment by ID → GET /api/comments/{id}
Update Comment → PUT /api/comments/{id}
Delete Comment → DELETE /api/comments/{id}

 */