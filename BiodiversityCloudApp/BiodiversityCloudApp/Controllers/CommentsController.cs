using Microsoft.AspNetCore.Http.HttpResults;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BiodiversityCloudApp.Controllers
{
    public class CommentsController : Controller
    {
        public readonly ApplicationDbContext _context;
        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a Comment → POST /api/comments
        
        // Get All Comments → GET /api/comments
        // Get Comment by ID → GET /api/comments/{id}
        // Update Comment → PUT /api/comments/{id}
        // Delete Comment → DELETE /api/comments/{id}

    }
}