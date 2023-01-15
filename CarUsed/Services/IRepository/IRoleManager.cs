using CarUsed.Models;

namespace CarUsed.Services.IRepository
{
    public interface IRoleManager
    {
        Task<bool> AddRoleAsync(string roleName);
        Task<bool> RemoveRoleAsync(string roleName);
        Task<bool> AddUserToRoleAsync(User user,Role role);
        Task<bool> RemoveUserFromRoleAsync(User user,Role role);
        Task<string?[]> UserGetAllRole(Guid userId);
    }
}
