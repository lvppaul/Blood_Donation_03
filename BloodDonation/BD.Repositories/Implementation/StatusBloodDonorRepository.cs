using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BD.Repositories.Implementation
{
    public class StatusBloodDonorRepository : IStatusBloodDonorRepository
    {
        private readonly BloodDonationDbContext _context;
        public StatusBloodDonorRepository(BloodDonationDbContext context)
        {
            _context = context;
        }

        public async Task<StatusBloodDonor> AddAsync(StatusBloodDonor status)
        {
            await _context.AddAsync(status);
            await _context.SaveChangesAsync();
            return status;
        }

        public async Task DeleteAsync(StatusBloodDonor status)
        {
            status.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StatusBloodDonor>> GetAllAsync()
        {
            return await _context.StatusBloodDonors
                  .Where(sn => sn.IsDeleted != true)
                  .ToListAsync();
        }

        public async Task<StatusBloodDonor?> GetByIdAsync(int status_donor_id)
        {
            return await _context.StatusBloodDonors
                .FirstOrDefaultAsync(s => s.StatusDonorId == status_donor_id && s.IsDeleted != true);
        }

        public async Task<StatusBloodDonor> UpdateAsync(StatusBloodDonor status)
        {
            _context.StatusBloodDonors.Update(status);
            await _context.SaveChangesAsync();
            return status;
        }

    }
}
