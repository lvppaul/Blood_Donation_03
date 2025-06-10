using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BD.Repositories.Implementation
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly BloodDonationDbContext _context;

        public NotificationRepository(BloodDonationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int notificationId)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == notificationId);
            if (notification != null)
            {
                notification.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Notifications.Where(n => (bool)!n.IsDeleted).ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(int notificationId)
        {
            var notification = await _context.Notifications.Include(u => u.User).Include(s => s.StatusNotification).Where(n => n.NotificationId == notificationId && (bool)!n.IsDeleted).FirstOrDefaultAsync();
            return notification;
        }

        public Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
