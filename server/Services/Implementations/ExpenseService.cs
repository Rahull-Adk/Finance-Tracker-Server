using server.DTOs;
using server.Models;
using server.Repositories.Interfaces;
using server.Services.ServiceInterfaces;

namespace server.Services.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUserService _userService;
        private readonly IAuthRepository _authRepository;
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IUserService userService, IAuthRepository authRepository, IExpenseRepository expenseRepository)
        {
            _authRepository = authRepository;
            _userService = userService;
            _expenseRepository = expenseRepository;
        }

        public async Task<Result<ExpenseModel>> AddExpenseAsync(ExpenseDTO expense)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser.Data is null)
            {
                return Result<ExpenseModel>.Error(404, "User not found");
            }

            var createdExpense = new ExpenseModel()
            {
               UserId = currentUser.Data.Id,
                Category = expense.Category,
                Description = expense.Description,
                Date = expense.Date,
                Amount = expense.Amount,
            };

            currentUser.Data.Expenses.Add(createdExpense);
            currentUser.Data.Balance -= createdExpense.Amount; 
            await _authRepository.UpdateUserAsync(currentUser.Data);
            await _expenseRepository.AddExpenseAsync(createdExpense);

            return Result<ExpenseModel>.Success(createdExpense, string.Empty);
        }

        public async Task<Result<ICollection<ExpenseModel>>> GetAllExpensesAsync()
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser.Data is null)
            {
                return Result<ICollection<ExpenseModel>>.Error(404, "User not found");
            }
            return Result<ICollection<ExpenseModel>>.Success(currentUser.Data.Expenses, string.Empty);
        }

        public async Task<Result<ExpenseModel>> GetExpenseByIdAsync(int id)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            var result = await _expenseRepository.GetCurrentUserExpenseByIdAsync(id, currentUser);
            var expense = result.Data;
            if (expense is null)
            {
                return Result<ExpenseModel>.Error(result.ErrorCode, result.ErrorMessage);
            }
            return Result<ExpenseModel>.Success(expense, string.Empty);
        }

        public async Task<Result<ExpenseModel>> UpdateExpenseAsync(int id, ExpenseDTO expense)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser.Data is null)
            {
                return Result<ExpenseModel>.Error(404, "User not found");
            }

            var result = await _expenseRepository.GetCurrentUserExpenseByIdAsync(id, currentUser);
            var selectedExpense = result.Data;
            if (selectedExpense is null)
            {
                return Result<ExpenseModel>.Error(404, "Expense with given Id not found");
            }

        
            currentUser.Data.Balance += selectedExpense.Amount;
            currentUser.Data.Balance -= expense.Amount;

            selectedExpense.Category = expense.Category;
            selectedExpense.Amount = expense.Amount;
            selectedExpense.Description = expense.Description;
            selectedExpense.Date = expense.Date;

            try
            {
                await _expenseRepository.UpdateExpenseAsync(selectedExpense);
                await _authRepository.UpdateUserAsync(currentUser.Data);
                return Result<ExpenseModel>.Success(selectedExpense, string.Empty);
            }
            catch (Exception ex)
            {
                return Result<ExpenseModel>.Error(500, ex.ToString());
            }
        }

        public async Task<Result<string>> DeleteExpenseByIdAsync(int id)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            var result = await _expenseRepository.GetCurrentUserExpenseByIdAsync(id, currentUser);
            var expense = result.Data;
            if (expense is null)
            {
                return Result<string>.Error(result.ErrorCode, result.ErrorMessage);
            }

            currentUser.Data.Balance += expense.Amount; 
            await _expenseRepository.DeleteExpenseAsync(expense);
            await _authRepository.UpdateUserAsync(currentUser.Data); 

            return Result<string>.Success(string.Empty, "Expense deleted successfully");
        }

        public async Task<Result<ICollection<ExpenseModel>>> GetExpensesByCategoryAsync(string category)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser.Data is null)
            {
                return Result<ICollection<ExpenseModel>>.Error(404, "User not found");
            }

            var expenses = await _expenseRepository.GetExpensesByCategoryAsync(category, currentUser);
            return Result<ICollection<ExpenseModel>>.Success(expenses, string.Empty);
        }
    }
}