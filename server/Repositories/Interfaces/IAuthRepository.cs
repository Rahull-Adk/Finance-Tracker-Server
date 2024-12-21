using server.Models;

namespace server.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task AddUserAsync(UserModel user);

        Task UpdateUserAsync(UserModel user);
        Task<Result<UserModel>> FindByUsernameAsync(string username);
        Task<Result<UserModel>> FindByIdAsync(string id);

    }
}
