using server.Models;

namespace server.Services.ServiceInterfaces
{
    public interface IUserService
    {
        public Task<Result<UserModel>> GetCurrentUserAsync();
    }
}
