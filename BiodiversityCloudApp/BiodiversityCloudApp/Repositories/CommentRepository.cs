using Microsoft.EntityFrameworkCore;

namespace BiodiversityCloudApp.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Comment>> GetByObservationIdAsync(Guid observationId)
        {
            return await _context.Comments
                .Where(c => c.ObservationId == observationId)
                .ToListAsync();
        }
    }
}
