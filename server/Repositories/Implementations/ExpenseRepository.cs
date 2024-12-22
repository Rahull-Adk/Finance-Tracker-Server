﻿using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.Repositories.Interfaces;

namespace server.Repositories.Implementations
{
    public class ExpenseRepository : IExpenseRepository
        {
            private readonly AppDbContext _db;
            public ExpenseRepository(AppDbContext db)
            {
                _db = db;
            }

            public async Task<Result<ExpenseModel>> GetCurrentUserExpenseByIdAsync(int id, Result<UserModel> currentUser)
            {
                var expense = await _db.Expenses.FirstOrDefaultAsync(e => e.UserId == currentUser.Data.Id && e.Id == id);
                if (expense is null)
                {
                    return Result<ExpenseModel>.Error(404, "Expense not found");
                }
                return Result<ExpenseModel>.Success(expense, string.Empty);
            }

            public async Task AddExpenseAsync(ExpenseModel expense)
            {
                await _db.Expenses.AddAsync(expense);
                await _db.SaveChangesAsync();
            }

            public async Task UpdateExpenseAsync(ExpenseModel expense)
            {
                _db.Expenses.Update(expense);
                await _db.SaveChangesAsync();
            }

            public async Task DeleteExpenseAsync(ExpenseModel expense)
            {
                _db.Expenses.Remove(expense);
                await _db.SaveChangesAsync();
            }

            public async Task<ICollection<ExpenseModel>> GetExpensesByCategoryAsync(string category, Result<UserModel> currentUser)
            {
                return await _db.Expenses.AsNoTracking().Where(e => e.UserId == currentUser.Data.Id && e.Category == category).ToListAsync();
            }
        }
}