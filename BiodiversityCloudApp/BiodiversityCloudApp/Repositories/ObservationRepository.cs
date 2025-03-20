using Microsoft.EntityFrameworkCore;

namespace BiodiversityCloudApp.Repositories
{
    public class ObservationRepository : GenericRepository<Observation>, IObservationRepository
    {
        public ObservationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Observation>> GetAllAsync()
        {
            return await _context.Observations
                .Include(o => o.Photos) // Ensure photos are loaded
                .ToListAsync();
        }

        public async Task<IEnumerable<Observation>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Observations
                .Include(o => o.Photos)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
    }
}
