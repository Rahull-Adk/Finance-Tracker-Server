using server.DTOs;
using server.Models;
using server.Repositories.Interfaces;
using server.Services.ServiceInterfaces;

namespace server.Services.Implementations
{
    public class IncomeService : IIncomeService
    {
        private readonly IUserService _userService;
        private readonly IAuthRepository _authRepository;
        private readonly IIncomeRepository _incomeRepository;
        public IncomeService(IUserService userService, IAuthRepository authRepository, IIncomeRepository incomeRepository)
        {
            _authRepository = authRepository;
            _userService = userService;
            _incomeRepository = incomeRepository;
        }

        public async Task<Result<IncomeModel>> AddIncomeAsync(IncomeDTO income)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser.Data is null)
            {
                return Result<IncomeModel>.Error(404, "User not found");
            }

            var existingIncome = currentUser.Data.Incomes.FirstOrDefault(i =>
                i.Amount == income.Amount &&
                i.Source == income.Source &&
                i.Description == income.Description &&
                i.Date == income.Date);

            if (existingIncome != null)
            {
                return Result<IncomeModel>.Error(409, "Duplicate income entry");
            }

            var createdIncome = new IncomeModel()
            {
                UserId = currentUser.Data.Id,
                Source = income.Source,
                Description = income.Description,
                Date = income.Date,
                Amount = income.Amount,
            };

            currentUser.Data.Incomes.Add(createdIncome);
            currentUser.Data.Balance += createdIncome.Amount;
            await _authRepository.UpdateUserAsync(currentUser.Data);
            await _incomeRepository.AddIncomeAsync(createdIncome);

            return Result<IncomeModel>.Success(createdIncome, string.Empty);
        }

        public async Task<Result<ICollection<IncomeModel>>> GetAllIncomeAsync()
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser.Data is null)
            {
                return Result<ICollection<IncomeModel>>.Error(currentUser.ErrorCode, "User not found");
            }
            return Result<ICollection<IncomeModel>>.Success(currentUser.Data.Incomes, string.Empty);
        }

        public async Task<Result<IncomeModel>> GetIncomeByIdAsync(int id)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            var result = await _incomeRepository.GetCurrentUserIncomeByIdAsync(id, currentUser);
            var income = result.Data;
            if (income is null)
            {
                return Result<IncomeModel>.Error(result.ErrorCode, result.ErrorMessage);
            }
            return Result<IncomeModel>.Success(income, string.Empty);
        }

        public async Task<Result<IncomeModel>> UpdateIncomeAsync(int id, IncomeDTO income)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser.Data is null)
            {
                return Result<IncomeModel>.Error(404, "User not found");
            }

            var result = await _incomeRepository.GetCurrentUserIncomeByIdAsync(id, currentUser);
            var selectedIncome = result.Data;
            if (selectedIncome is null)
            {
                return Result<IncomeModel>.Error(404, "Income with given Id not found");
            }

            // Update balance
            currentUser.Data.Balance -= selectedIncome.Amount;
            currentUser.Data.Balance += income.Amount;

            selectedIncome.Source = income.Source;
            selectedIncome.Amount = income.Amount;
            selectedIncome.Description = income.Description;
            selectedIncome.Date = income.Date;

            try
            {
                await _incomeRepository.UpdateIncomeAsync(selectedIncome);
                await _authRepository.UpdateUserAsync(currentUser.Data);
                return Result<IncomeModel>.Success(selectedIncome, string.Empty);
            }
            catch (Exception ex)
            {
                return Result<IncomeModel>.Error(500, ex.ToString());
            }
        }

        public async Task<Result<string>> DeleteIncomeByIdAsync(int id)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            var result = await _incomeRepository.GetCurrentUserIncomeByIdAsync(id, currentUser);
            var income = result.Data;
            if (income is null)
            {
                return Result<string>.Error(result.ErrorCode, result.ErrorMessage);
            }

            currentUser.Data.Balance -= income.Amount;
            await _incomeRepository.DeleteIncomeAsync(income);
            await _authRepository.UpdateUserAsync(currentUser.Data);
            return Result<string>.Success(string.Empty, "Income deleted successfully");
        }
    }
}