using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Services.Interfaces
{
    public interface IDonationHistoryService
    {
        public Task<IEnumerable<DonationHistoryResponse>> GetAllAsync();
        public Task<DonationHistoryResponse?> GetByIdAsync(int donationHistoryId);
        public Task<DonationHistoryResponse> AddAsync(DonationHistoryRequest donationHistory);
        public Task<DonationHistoryResponse?> UpdateAsync(int donationHistoryId, DonationHistoryRequest donationHistory);
        public Task<bool> UpdateStatusAsync(int donationId, DonationStatus status);
        public Task<IEnumerable<DonationHistoryResponse>> GetByStatusAsync(DonationStatus status);
        public Task<IEnumerable<DonationHistoryResponse>> GetByUserIdAsync(int userId);
        public Task DeleteAsync(int donationHistoryId);
    }
}
