using server.DTOs;
using server.Models;

namespace server.Services.ServiceInterfaces
{
    public interface IExpenseService
    {
        Task<Result<ExpenseModel>> AddExpenseAsync(ExpenseDTO expense);
        Task<Result<ICollection<ExpenseModel>>> GetAllExpensesAsync();
        Task<Result<ExpenseModel>> GetExpenseByIdAsync(int id);
        Task<Result<ExpenseModel>> UpdateExpenseAsync(int id, ExpenseDTO expense);
        Task<Result<string>> DeleteExpenseByIdAsync(int id);
        Task<Result<ICollection<ExpenseModel>>> GetExpensesByCategoryAsync(string category);
    }
}
