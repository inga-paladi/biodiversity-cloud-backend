using Microsoft.EntityFrameworkCore;
using BiodiversityCloudApp.Models;

namespace BiodiversityCloudApp.Repositories;
public class PhotoRepository(ApplicationDbContext context) : GenericRepository<Photo>(context), IPhotoRepository
{
}
