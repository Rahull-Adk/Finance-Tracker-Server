using server.Models;

namespace server.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<Result<ExpenseModel>> GetCurrentUserExpenseByIdAsync(int id, Result<UserModel> currentUser);
<<<<<<< HEAD
        Task<ExpenseModel> AddExpenseAsync(ExpenseModel expense);
        Task<ExpenseModel> UpdateExpenseAsync(ExpenseModel expense);
=======
        Task AddExpenseAsync(ExpenseModel expense);
        Task UpdateExpenseAsync(ExpenseModel expense);
>>>>>>> 2fe5ab6dbac881f8192bf317361ac707ee0c9ea6
        Task DeleteExpenseAsync(ExpenseModel expense);
        Task<ICollection<ExpenseModel>> GetExpensesByCategoryAsync(string category, Result<UserModel> currentUser);
    }
}
