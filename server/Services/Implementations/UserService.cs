using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.Repositories.Interfaces;
using server.Services.ServiceInterfaces;
using System.Security.Claims;

namespace server.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAuthRepository _authRepository;
        public UserService(AppDbContext db, IHttpContextAccessor httpContext, IAuthRepository authRepository)
        {
            _db = db;
            _httpContext = httpContext;
            _authRepository = authRepository;
        }
        public async Task<Result<UserModel>> GetCurrentUserAsync()
        {
            if (!_httpContext.HttpContext!.User.Identity?.IsAuthenticated ?? false)
            {
                return Result<UserModel>.Error(401, "Unauthorized access");
            }

            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Result<UserModel>.Error(401, "Invalid Token");
            }

            var currentUser = await _db.Users.Include(u => u.Incomes).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (currentUser is null)
            {
                return Result<UserModel>.Error(404, "User not found");
            }
            var balance = currentUser.Incomes.Sum(i => i.Amount);
            currentUser.Balance = balance;
            await _db.SaveChangesAsync();
            return Result<UserModel>.Success(currentUser, string.Empty);
        }

    }
}
