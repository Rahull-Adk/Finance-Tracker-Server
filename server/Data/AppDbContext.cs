using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) 
        {
            
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ExpenseModel> Expenses { get; set; }
        public DbSet<IncomeModel> Incomes { get; set; }
    }
}
