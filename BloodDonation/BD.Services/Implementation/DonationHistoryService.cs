using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Mappers;
using BD.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Services.Implementation
{
    public class DonationHistoryService : IDonationHistoryService
    {
        private readonly IDonationHistoryRepository _donationHistoryRepository;
        public DonationHistoryService(IDonationHistoryRepository donationHistoryRepository)
        {
            _donationHistoryRepository = donationHistoryRepository;
        }
        public async Task<DonationHistoryResponse> AddAsync(DonationHistoryRequest donationHistory)
        {
            var donationHistoryEntity = DonationHistoryMapper.ToEntity(donationHistory);
            await _donationHistoryRepository.AddDonationHistoryAsync(donationHistoryEntity);
            return DonationHistoryMapper.ToResponse(donationHistoryEntity);
        }

        public async Task DeleteAsync(int donationHistoryId)
        {
            var donationHistory = await _donationHistoryRepository.GetDonationHistoryByIdAsync(donationHistoryId);
            if (donationHistory == null)
            {
                throw new Exception("Donation History not found");
            }

            await _donationHistoryRepository.DeleteDonationHistoryAsync(donationHistory);
        }

        public async Task<IEnumerable<DonationHistoryResponse>> GetAllAsync()
        {
            var donationHistories = await _donationHistoryRepository.GetAllDonationHistoryAsync();
            return donationHistories.Select(DonationHistoryMapper.ToResponse);
        }

        public async Task<DonationHistoryResponse?> GetByIdAsync(int donationHistoryId)
        {
            var donationHistory = await _donationHistoryRepository.GetDonationHistoryByIdAsync(donationHistoryId);
            return donationHistory == null ? null : DonationHistoryMapper.ToResponse(donationHistory);
        }

        public async Task<DonationHistoryResponse?> UpdateAsync(int donationHistoryId, DonationHistoryRequest donationHistory)
        {
            var existing = await _donationHistoryRepository.GetDonationHistoryByIdAsync(donationHistoryId);
            if (existing == null)
            {
                return null;
            }

            existing.RequestId = donationHistory.RequestId;
            existing.FacilityId = donationHistory.FacilityId;
            existing.Amount = donationHistory.Amount;
            existing.DonationDate = donationHistory.DonationDate;
            existing.BloodType = donationHistory.BloodType;
            existing.ComponentType = donationHistory.ComponentType;

            var updated = await _donationHistoryRepository.UpdateDonationHistoryAsync(existing);

            return DonationHistoryMapper.ToResponse(updated);
        }
    }
}
