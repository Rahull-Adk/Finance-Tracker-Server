using server.Models;

namespace server.Repositories.Interfaces
{
    public interface IIncomeRepository
    {
        Task<Result<IncomeModel>> GetCurrentUserIncomeByIdAsync(int id, Result<UserModel> currentUser);
<<<<<<< HEAD
        Task<IncomeModel> AddIncomeAsync(IncomeModel income);
        Task<IncomeModel> UpdateIncomeAsync(IncomeModel income);
=======
        Task AddIncomeAsync(IncomeModel income);
        Task UpdateIncomeAsync(IncomeModel income);
>>>>>>> 2fe5ab6dbac881f8192bf317361ac707ee0c9ea6
        Task DeleteIncomeAsync(IncomeModel income);
    }
}