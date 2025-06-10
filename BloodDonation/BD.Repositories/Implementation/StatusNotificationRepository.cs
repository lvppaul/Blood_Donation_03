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

        public async Task AddAsync(StatusNotification statusNotification)
        {
            _context.Add(statusNotification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var statusNotification = await _context.StatusNotifications.FindAsync(id);
            if (statusNotification != null)
            {
                statusNotification.IsDeleted = true; // Soft delete
                await _context.SaveChangesAsync();
            }
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

        public async Task UpdateAsync(StatusNotification statusNotification)
        {
            _context.StatusNotifications.Update(statusNotification);
            await _context.SaveChangesAsync();
        }
    }
}
