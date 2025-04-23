using BiodiversityCloudApp.Models;

namespace BiodiversityCloudApp.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetByEmailAsync(string email);
}
