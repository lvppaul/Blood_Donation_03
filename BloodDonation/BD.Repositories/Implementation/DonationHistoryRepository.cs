using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.Entities;
using BD.Repositories.Models.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Implementation
{
    public class DonationHistoryRepository : IDonationHistoryRepository
    {
        private readonly BloodDonationDbContext _context;
        public DonationHistoryRepository(BloodDonationDbContext context)
        {
            _context = context;
        }
        public async Task<DonationHistory> AddDonationHistoryAsync(DonationHistory donationHistory)
        {
            await _context.DonationHistories.AddAsync(donationHistory);
            await _context.SaveChangesAsync();

            return donationHistory;
        }

        public async Task DeleteDonationHistoryAsync(DonationHistory donationHistory)
        {
            donationHistory.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DonationHistory>> GetAllDonationHistoryAsync()
        {
            var histories = await _context.DonationHistories
            .Include(dh => dh.Facility)
            .Include(dh => dh.User)
            .Include(dh => dh.Request)
                .Where(dh => dh.IsDeleted != true)
                
            .ToListAsync();

            return histories;
        }        public async Task<DonationHistory?> GetDonationHistoryByIdAsync(int id)
        {
            return await _context.DonationHistories
                .Include(dh => dh.User)
                    .ThenInclude(u => u.Role)
                .Include(dh => dh.Facility)
                .FirstOrDefaultAsync(dh => dh.DonationId == id && dh.IsDeleted != true);
        }

        public async Task<IEnumerable<DonationHistory>> GetByUserIdAsync(int userId)
        {
            return await _context.DonationHistories
                .Include(dh => dh.Facility)
                .Where(dh => dh.UserId == userId && dh.IsDeleted != true)
                .OrderByDescending(dh => dh.DonationDate)
                .ToListAsync();
        }

        public async Task<DonationHistory?> GetLatestDonationByUserIdAsync(int userId)
        {
            return await _context.DonationHistories
                .Include(dh => dh.Facility)
                .Where(dh => dh.UserId == userId && dh.IsDeleted != true)
                .OrderByDescending(dh => dh.DonationDate)
                .FirstOrDefaultAsync();
        }

        public async Task<DonationHistory> UpdateDonationHistoryAsync(DonationHistory donationHistory)
        {
            _context.DonationHistories.Update(donationHistory);
            await _context.SaveChangesAsync();
            return donationHistory;
        }

        public async Task<bool> UpdateDonationStatusAsync(int donationId, DonationStatus status)
        {
            var donation = await _context.DonationHistories
                .FirstOrDefaultAsync(dh => dh.DonationId == donationId && dh.IsDeleted != true);

            if (donation == null) return false;

            donation.Status = status;

            // Update timestamps based on status
            switch (status)
            {
                case DonationStatus.Confirmed:
                    donation.ConfirmedDate = DateTime.Now;
                    break;
                case DonationStatus.Donated:
                    donation.DonationDate = DateTime.Now; // DonationDate represents completion time
                    break;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DonationHistory>> GetByStatusAsync(DonationStatus status)
        {
            return await _context.DonationHistories
                .Include(dh => dh.Facility)
                .Include(dh => dh.User)
                .Include(dh => dh.Request)
                .Where(dh => dh.Status == status && dh.IsDeleted != true)
                .OrderByDescending(dh => dh.CreatedDate)
                .ToListAsync();
        }
    }
}
