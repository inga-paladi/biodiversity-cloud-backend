using BiodiversityCloudApp.Models;

namespace BiodiversityCloudApp.Repositories;

public interface IObservationRecordRepository : IGenericRepository<ObservationRecord>
{
    Task<ObservationRecord?> GetRecordAsync(Guid recordId);
    Task<IEnumerable<ObservationRecord>> GetAllRecordsAsync(Guid observationId);
}
