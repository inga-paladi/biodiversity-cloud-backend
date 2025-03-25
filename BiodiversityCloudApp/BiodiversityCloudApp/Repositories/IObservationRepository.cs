namespace BiodiversityCloudApp.Repositories
{
    public interface IObservationRepository : IGenericRepository<Observation>
    {
        Task<IEnumerable<Observation>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Observation>> GetUpdatedSinceAsync(DateTime since);
    }

}
