using Microsoft.EntityFrameworkCore;
using BiodiversityCloudApp.Models;

namespace BiodiversityCloudApp.Repositories;

public class ObservationRecordRepository(ApplicationDbContext context) : GenericRepository<ObservationRecord>(context), IObservationRecordRepository
{
    public async Task<ObservationRecord?> GetRecordAsync(Guid recordId)
    {
        return await _context.ObservationRecords
            .Include(r => r.Photos)
            .FirstOrDefaultAsync(m => m.Id == recordId);
    }

    public async Task<IEnumerable<ObservationRecord>> GetAllRecordsAsync(Guid observationId)
    {
        return await _context.ObservationRecords
            .Where(m => m.ObservationId == observationId)
            .Include(r => r.Photos)
            .ToListAsync();
    }
}
