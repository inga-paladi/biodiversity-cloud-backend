using BiodiversityCloudApp.Models;

namespace BiodiversityCloudApp.Repositories;

public interface IObservationRepository : IGenericRepository<Observation>
{
    Task<Observation?> GetObservationAsync(Guid observationId);
    Task<IEnumerable<Observation>> GetObservationsAsync(Guid userId);
}
