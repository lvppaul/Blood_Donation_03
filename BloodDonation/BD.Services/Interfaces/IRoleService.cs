using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<string>> GetAllRolesAsync();
        Task<bool> RoleExistsAsync(string roleName);
        Task AddRoleAsync(string roleName);
        Task RemoveRoleAsync(string roleName);
        Task<bool> IsUserInRoleAsync(int userId, string roleName);
        Task AddUserToRoleAsync(int userId, string roleName);
        Task RemoveUserFromRoleAsync(int userId, string roleName);
    }
}
