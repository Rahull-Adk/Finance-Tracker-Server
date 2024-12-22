using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.Repositories.Interfaces;

namespace server.Repositories.Implementations
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly AppDbContext _db;
        public IncomeRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<IncomeModel>> GetCurrentUserIncomeByIdAsync(int id, Result<UserModel> currentUser)
        {
            var income = await _db.Incomes.AsNoTracking().FirstOrDefaultAsync(i => i.UserId == currentUser.Data.Id && i.Id == id);
            if (income is null)
            {
                return Result<IncomeModel>.Error(404, "Income not found");
            }
            return Result<IncomeModel>.Success(income, string.Empty);
        }

<<<<<<< HEAD
        public async Task<IncomeModel> AddIncomeAsync(IncomeModel income)
        {
            await _db.Incomes.AddAsync(income);
            await _db.SaveChangesAsync();
            return income;
        }

        public async Task<IncomeModel> UpdateIncomeAsync(IncomeModel income)
=======
        public async Task AddIncomeAsync(IncomeModel income)
        {
            await _db.Incomes.AddAsync(income);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateIncomeAsync(IncomeModel income)
>>>>>>> 2fe5ab6dbac881f8192bf317361ac707ee0c9ea6
        {
            _db.Incomes.Update(income);
            await _db.SaveChangesAsync();
            return income;
        }

        public async Task DeleteIncomeAsync(IncomeModel income)
        {
            _db.Incomes.Remove(income);
            await _db.SaveChangesAsync();
        }
    }
}