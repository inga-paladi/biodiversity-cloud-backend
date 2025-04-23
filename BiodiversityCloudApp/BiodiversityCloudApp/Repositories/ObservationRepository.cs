using BiodiversityCloudApp.Models;
using BiodiversityCloudApp.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BiodiversityCloudApp.Repositories;

public class ObservationRepository(ApplicationDbContext context) : GenericRepository<Observation>(context), IObservationRepository
{
    public async Task<Observation?> GetObservationAsync(Guid observationId)
    {
        var observation = await _context.Observations
            .Include(o => o.Records)
            .FirstOrDefaultAsync(o => o.Id == observationId);

        if (observation != null)
            observation.RecordIds = [.. observation.Records.Select(r => r.Id)];

        return observation;
    }

    public async Task<IEnumerable<Observation>> GetObservationsAsync(Guid userId)
    {
        var observations = await _context.Observations
            .Include(o => o.Records)
            .Where(o => o.UserId == userId)
            .ToListAsync();

        foreach (var observation in observations)
            observation.RecordIds = [.. observation.Records.Select(r => r.Id)];

        return observations;
    }
}
