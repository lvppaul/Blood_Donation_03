using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BD.Repositories.Implementation
{
    public class StatusBloodRequestRepository : IStatusBloodRequestRepository
    {
        private readonly BloodDonationDbContext _context;
        public StatusBloodRequestRepository(BloodDonationDbContext context)
        {
            _context = context;
        }

        public async Task<StatusBloodRequest> AddAsync(StatusBloodRequest status)
        {
            await _context.StatusBloodRequests.AddAsync(status);
            await _context.SaveChangesAsync();
            return status;
        }

        public async Task DeleteAsync(StatusBloodRequest status)
        {
            status.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StatusBloodRequest>> GetAllAsync()
        {
            return await _context.StatusBloodRequests
                    .Where(sn => sn.IsDeleted != true)
                    .ToListAsync();
        }

        public async Task<StatusBloodRequest?> GetByIdAsync(int status_request_id)
        {
            return await _context.StatusBloodRequests
                        .FirstOrDefaultAsync(s => s.StatusRequestId == status_request_id && s.IsDeleted != true);
        }

        public async Task<StatusBloodRequest> UpdateAsync(StatusBloodRequest status)
        {
            _context.StatusBloodRequests.Update(status);
            await _context.SaveChangesAsync();
            return status;
        }
    }
}
