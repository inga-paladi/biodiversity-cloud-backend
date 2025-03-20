using Microsoft.EntityFrameworkCore;

namespace BiodiversityCloudApp.Repositories
{
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        private readonly ApplicationDbContext _context;
        public PhotoRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Photo>> GetByObservationIdAsync(Guid observationId)
        {
            return await _context.Photos
                .Where(p => p.ObservationId == observationId)
                .ToListAsync();
        }
        public async Task AddAsync(Photo photo)
        {
            await _context.Photos.AddAsync(photo);
            await _context.SaveChangesAsync(); // Ensure data is saved
        }
        public async Task UpdateAsync(Photo photo)
        {
            _context.Photos.Update(photo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Photo photo)
        {
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
        }



    }
}
