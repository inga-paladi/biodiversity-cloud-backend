namespace BiodiversityCloudApp.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByObservationIdAsync(Guid observationId);
    }
}
