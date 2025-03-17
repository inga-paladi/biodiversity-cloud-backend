namespace BiodiversityCloudApp.Repositories
{
    public interface IObservationRepository : IGenericRepository<Observation>
    {
        Task<IEnumerable<Observation>> GetByUserIdAsync(Guid userId);
    }

}
