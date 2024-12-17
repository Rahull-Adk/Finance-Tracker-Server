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

        public async Task<UserModel> FindByUsernameAsync(string username)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == username);
            return user;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var users = await _db.Users.ToListAsync();
            return users;
        }
    }
}
