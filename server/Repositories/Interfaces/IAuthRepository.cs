using server.Models;

namespace server.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task AddUserAsync(UserModel user);
        Task<UserModel> FindByUsernameAsync(string username);

        Task<List<UserModel>> GetAllUsersAsync();
    }
}
