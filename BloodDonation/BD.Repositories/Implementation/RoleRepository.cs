using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly BloodDonationDbContext _context;
        public RoleRepository(BloodDonationDbContext context)
        {
            _context = context;
        }
        public async Task<Role> AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task DeleteAsync(Role role)
        {
            role.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles
                .Where(r => r.IsDeleted != true)
                .ToListAsync();
        }

        public Task<Role?> GetByIdAsync(int id)
        {
            return _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id && r.IsDeleted != true);
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return role;
        }
    }
}
