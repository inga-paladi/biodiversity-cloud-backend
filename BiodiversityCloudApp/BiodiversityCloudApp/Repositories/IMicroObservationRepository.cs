using BiodiversityCloudApp.Models;

namespace BiodiversityCloudApp.Repositories
{
    public interface IMicroObservationRepository : IGenericRepository<MicroObservation>
    {
        Task<IEnumerable<MicroObservation>> GetByObservationIdAsync(Guid observationId);
    }
}