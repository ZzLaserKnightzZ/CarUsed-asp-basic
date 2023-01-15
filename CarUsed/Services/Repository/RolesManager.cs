using CarUsed.Models;
using CarUsed.Services.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarUsed.Services.Repository
{
    public class RolesManager : IRoleManager
    {
        private readonly AppDbContext _appDbContext;

        public RolesManager(AppDbContext appDb)
        {
            _appDbContext = appDb;
        }
        public async Task<bool> AddRoleAsync(string roleName)
        {
            try
            {
                await _appDbContext.Roles.AddAsync(new Role { Name = roleName });
                await _appDbContext.SaveChangesAsync();
                _appDbContext.Dispose();
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<bool> AddUserToRoleAsync(User user, Role role)
        {
            try
            {
                await _appDbContext.UsersRoles.AddAsync(new UserRole { Role = role, User = user });
                await _appDbContext.SaveChangesAsync();
                _appDbContext.Dispose();
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public async Task<bool> RemoveRoleAsync(string roleName)
        {
            try
            {
                var role = await _appDbContext.Roles.FirstOrDefaultAsync(x => x.Name.Equals(roleName));
                if (role != null)
                {
                    _appDbContext.Roles.Remove(role);
                    await _appDbContext.SaveChangesAsync();
                    _appDbContext.Dispose();
                    return true;
                }

            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public async Task<bool> RemoveUserFromRoleAsync(User user, Role role)
        {
            try
            {
                var userRoles = await _appDbContext.UsersRoles.FirstOrDefaultAsync(x => x.Role.RoleId == role.RoleId && x.UserId.Equals(user.UserId));
                if (userRoles != null)
                {
                    _appDbContext.UsersRoles.Remove(userRoles);
                    await _appDbContext.SaveChangesAsync();
                    _appDbContext.Dispose();
                    return true;
                }

            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public Task<string?[]> UserGetAllRole(Guid userId)
        {
            return Task.FromResult(_appDbContext.UsersRoles.AsNoTracking().Where(x => x.UserId.Equals(userId)).Include(x => x.Role).Select(x => x.Role.Name).ToArray());
        }
    }
}
