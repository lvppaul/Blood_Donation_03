using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Interfaces
{    public interface IDonationHistoryRepository
    {
        Task<IEnumerable<DonationHistory>> GetAllDonationHistoryAsync();
        Task<DonationHistory?> GetDonationHistoryByIdAsync(int id);
        Task<IEnumerable<DonationHistory>> GetByUserIdAsync(int userId);
        Task<DonationHistory?> GetLatestDonationByUserIdAsync(int userId);
        Task<DonationHistory> AddDonationHistoryAsync(DonationHistory donationHistory);
        Task<DonationHistory> UpdateDonationHistoryAsync(DonationHistory donationHistory);
        Task<bool> UpdateDonationStatusAsync(int donationId, DonationStatus status);
        Task<IEnumerable<DonationHistory>> GetByStatusAsync(DonationStatus status);
        Task DeleteDonationHistoryAsync(DonationHistory donationHistory);
    }
}
