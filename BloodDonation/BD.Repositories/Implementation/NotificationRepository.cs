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

        public async Task<Notification> AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task DeleteAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Notifications.Where(n => n.IsDeleted != true).ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(int notificationId)
        {
            var notification = await _context.Notifications.Where(n => n.NotificationId == notificationId && !n.IsDeleted != true).FirstOrDefaultAsync();
            return notification;
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
        {
            var notificationList = await _context.Notifications.Include(n => n.User)
                .Where(n => n.UserId == userId && n.IsDeleted != true)
                .ToListAsync();
            return notificationList;
        }

        public async Task<Notification> UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
    }
}
