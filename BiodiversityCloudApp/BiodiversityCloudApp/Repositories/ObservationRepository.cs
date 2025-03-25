using Microsoft.EntityFrameworkCore;

namespace BiodiversityCloudApp.Repositories
{
    public class ObservationRepository(ApplicationDbContext context) : GenericRepository<Observation>(context), IObservationRepository
    {
        public async Task<IEnumerable<Observation>> GetUpdatedSinceAsync(DateTime since)
        {
            return await _context.Observations
                .Where(o => o.UpdatedAt > since)
                .ToListAsync();
        }
    }
}
