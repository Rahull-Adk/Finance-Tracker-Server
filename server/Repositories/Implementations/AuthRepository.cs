using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.Repositories.Interfaces;

namespace server.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _db;
        public AuthRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddUserAsync(UserModel user) { 
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(UserModel user)
        {
             _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<Result<UserModel>> FindByUsernameAsync(string username)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return Result<UserModel>.Error(404, "User not found");
            }
            return Result<UserModel>.Success(user, string.Empty);
        }
        public async Task<Result<UserModel>> FindByIdAsync(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id.ToString() == id);
            if (user == null) {
                return Result<UserModel>.Error(404, "User not found");
            }
            return Result<UserModel>.Success(user, string.Empty);
        }

    }
}
