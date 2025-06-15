using BD.Repositories.Interfaces;
using BD.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public Task AddRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task AddUserToRoleAsync(int userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserInRoleAsync(int userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserFromRoleAsync(int userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RoleExistsAsync(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
