using server.Models;

namespace server.Repositories.Interfaces
{
    public interface IIncomeRepository
    {
        Task<Result<IncomeModel>> GetCurrentUserIncomeByIdAsync(int id, Result<UserModel> currentUser);
        Task AddIncomeAsync(IncomeModel income);
        Task UpdateIncomeAsync(IncomeModel income);
        Task DeleteIncomeAsync(IncomeModel income);
    }
}