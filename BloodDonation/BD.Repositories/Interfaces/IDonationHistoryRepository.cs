using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Interfaces
{
    public interface IDonationHistoryRepository
    {
        Task<IEnumerable<DonationHistory>> GetAllDonationHistoryAsync();
        Task<DonationHistory> GetDonationHistoryByIdAsync(int id);
        Task<DonationHistory> AddDonationHistoryAsync(DonationHistory donationHistory);
        Task<DonationHistory> UpdateDonationHistoryAsync(DonationHistory donationHistory);
        Task DeleteDonationHistoryAsync(int id);
    }
}
