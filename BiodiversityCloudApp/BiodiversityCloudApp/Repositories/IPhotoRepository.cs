namespace BiodiversityCloudApp.Repositories
{
    public interface IPhotoRepository : IGenericRepository<Photo>
    {
        Task<IEnumerable<Photo>> GetByObservationIdAsync(Guid observationId);
        Task UpdateAsync(Photo photo);
        Task DeleteAsync(Photo photo);
    }
}
