using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BD.Repositories.Implementation
{
    public class BloodCompatibilityRepository : IBloodCompatibilityRepository
    {
        private readonly BloodDonationDbContext _context;

        public BloodCompatibilityRepository(BloodDonationDbContext context)
        {
            _context = context;
        }
        public async Task<BloodCompatibility> AddAsync(BloodCompatibility bloodCompatibility)
        {
            await _context.BloodCompatibilities.AddAsync(bloodCompatibility);
            await _context.SaveChangesAsync();
            return bloodCompatibility;
        }

        public async Task DeleteAsync(BloodCompatibility bloodCompatibility)
        {
            bloodCompatibility.IsDeleted = true;
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<BloodCompatibility>> GetAllAsync()
        {
            return await _context.BloodCompatibilities.Where(b => b.IsDeleted != true).ToListAsync();
        }

        public async Task<BloodCompatibility?> GetByIdAsync(int bloodCompatibilityId)
        {
            return await _context.BloodCompatibilities
                .FirstOrDefaultAsync(b => b.CompatibilityId == bloodCompatibilityId && b.IsDeleted != true);
        }

        public async Task<BloodCompatibility> UpdateAsync(BloodCompatibility bloodCompatibility)
        {
            _context.BloodCompatibilities.Update(bloodCompatibility);
            await _context.SaveChangesAsync();
            return bloodCompatibility;
        }
    }
}
