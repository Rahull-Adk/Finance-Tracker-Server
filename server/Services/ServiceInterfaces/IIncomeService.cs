using server.DTOs;
using server.Models;
using System.Globalization;

namespace server.Services.ServiceInterfaces
{
    public interface IIncomeService
    {
        public Task<Result<IncomeModel>> AddIncomeAsync(IncomeDTO income);
        public Task<Result<ICollection<IncomeModel>>> GetAllIncomeAsync();

        public Task<Result<IncomeModel>> GetIncomeByIdAsync(int id);
        public Task<Result<IncomeModel>> UpdateIncomeAsync(int id, IncomeDTO income);
        public Task<Result<string>> DeleteIncomeByIdAsync(int id);
    }
}
