using Api.Application.Database;
using Api.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Services.Auth
{
    public class RoleService
    {
        private readonly DatabaseContext _db;

        public RoleService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<Role>> FindUserRolesAsync(string userId)
        {
            var roles = await _db.Roles.Where(role => role.UserRoles.Any(x => x.UserId == userId)).ToListAsync();
            return roles;
        }

        public async Task<bool> IsUserInRole(string userId, string roleName)
        {
            var userRolesQuery = from role in _db.Roles
                                 where role.Name == roleName
                                 from user in role.UserRoles
                                 where user.UserId == userId
                                 select role;
            var userRole = await userRolesQuery.FirstOrDefaultAsync();
            return userRole != null;
        }

        public async Task<List<User>> FindUsersInRoleAsync(string roleName)
        {
            var roleUserIdsQuery = from role in _db.Roles
                                   where role.Name == roleName
                                   from user in role.UserRoles
                                   select user.UserId;
            return await _db.Users.Where(user => roleUserIdsQuery.Contains(user.UserId))
                .ToListAsync();
        }
    }
}
