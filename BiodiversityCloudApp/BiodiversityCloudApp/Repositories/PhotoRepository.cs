using Microsoft.EntityFrameworkCore;

namespace BiodiversityCloudApp.Repositories
{
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Photo>> GetByObservationIdAsync(Guid observationId)
        {
            return await _context.Photos
                .Where(p => p.ObservationId == observationId)
                .ToListAsync();
        }
    }
}
