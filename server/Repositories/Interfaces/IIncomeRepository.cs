using server.Models;

namespace server.Repositories.Interfaces
{
    public interface IIncomeRepository
    {
        public Task<Result<IncomeModel>> GetCurrentUserIncomeByIdAsync(int id, Result<UserModel> currentUser);
        public Task UpdateIncomeAsync(IncomeModel income);
        public Task DeleteIncomeAsync(IncomeModel income);
    }
}
