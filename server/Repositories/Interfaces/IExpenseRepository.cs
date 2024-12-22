using server.Models;

namespace server.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<Result<ExpenseModel>> GetCurrentUserExpenseByIdAsync(int id, Result<UserModel> currentUser);
        Task AddExpenseAsync(ExpenseModel expense);
        Task UpdateExpenseAsync(ExpenseModel expense);
        Task DeleteExpenseAsync(ExpenseModel expense);
        Task<ICollection<ExpenseModel>> GetExpensesByCategoryAsync(string category, Result<UserModel> currentUser);
    }
}
