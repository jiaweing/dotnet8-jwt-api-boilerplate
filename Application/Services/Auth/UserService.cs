using Api.Application.Database;
using Api.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Api.Application.Services.Auth
{
    public class UserService
    {
        private readonly DatabaseContext _db;

        public UserService(
            DatabaseContext db
        )
        {
            _db = db;
        }

        public async Task<User?> FindUserAsync(string userId)
        {
            return await _db.Users.FindAsync(userId);
        }

        public async Task<User?> FindUserAsync(string username, string password)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.EmailAddress == username && x.Password == password);
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.EmailAddress == email);
        }

        public async Task<User> CreateUserAsync(User user, int roleId)
        {
            var addedUser = await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            await _db.UserRoles.AddAsync(new UserRole { RoleId = roleId, User = user });
            await _db.SaveChangesAsync();

            return addedUser.Entity;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _db.Users
                    .ToListAsync();
        }

        public string GetSha256Hash(string input)
        {
            using (var hashAlgorithm = SHA256.Create())
            {
                var byteValue = Encoding.UTF8.GetBytes(input);
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }
    }
}
