using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BD.Repositories.Implementation
{
    public class StatusNotificationRepository : IStatusNotificationRepository
    {
        private readonly BloodDonationDbContext _context;
        public StatusNotificationRepository(BloodDonationDbContext context)
        {
            _context = context;
        }

        public async Task<StatusNotification> AddAsync(StatusNotification statusNotification)
        {
            await _context.AddAsync(statusNotification);
            await _context.SaveChangesAsync();
            return statusNotification;
        }

        public async Task DeleteAsync(StatusNotification statusNotification)
        {
            statusNotification.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<StatusNotification>> GetAllAsync()
        {
            return await _context.StatusNotifications
                 .Where(sn => sn.IsDeleted != true)
                 .ToListAsync();
        }

        public async Task<StatusNotification?> GetByIdAsync(int id)
        {
            return await _context.StatusNotifications
                .FirstOrDefaultAsync(sn => sn.StatusNotificationId == id && sn.IsDeleted != true);

        }

        public async Task<StatusNotification> UpdateAsync(StatusNotification statusNotification)
        {
            _context.StatusNotifications.Update(statusNotification);
            await _context.SaveChangesAsync();
            return statusNotification;
        }
    }
}
