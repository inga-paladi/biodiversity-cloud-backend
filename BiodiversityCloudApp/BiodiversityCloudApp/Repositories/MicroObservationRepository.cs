using BiodiversityCloudApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BiodiversityCloudApp.Repositories
{
    public class MicroObservationRepository : GenericRepository<MicroObservation>, IMicroObservationRepository
    {
        private readonly ApplicationDbContext _context;

        public MicroObservationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MicroObservation>> GetByObservationIdAsync(Guid observationId)
        {
            return await _context.MicroObservations
                .Include(m => m.Animal)
                .Where(m => m.ObservationId == observationId)
                .ToListAsync();
        }
    }
}
