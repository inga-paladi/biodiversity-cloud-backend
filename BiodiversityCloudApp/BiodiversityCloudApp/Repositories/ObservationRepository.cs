using BiodiversityCloudApp.Models.Enums;
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
        public async Task<IEnumerable<Observation>> GetUpdatedSinceAndStatusAsync(DateTime since, ObservationStatus observationStatus)
        {
            return await _context.Observations
                .Where(o => o.UpdatedAt >= since && o.ObservationStatus == observationStatus)
                .ToListAsync();
        }
        public async Task<int> GetUniqueSpeciesCountByUserAsync(Guid userId)
        {
            return await _context.Observations
                .Where(o => o.UserId == userId)
                .Select(o => o.Species)
                .Distinct()
                .CountAsync();
        }
    }
}
