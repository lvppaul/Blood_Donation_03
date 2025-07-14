using BD.Repositories.Implementation;
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
    public class DonorAvailabilityService : IDonorAvailabilityService
    {
        private readonly IDonorAvailabilityRepository _repository;
        public DonorAvailabilityService(IDonorAvailabilityRepository repository)
        {
            _repository = repository;
        }
        public async Task<DonorAvailabilityResponse> AddAsync(DonorAvailabilityRequest donorAvailability)
        {
            var entity = DonorAvailabilityMapper.ToEntity(donorAvailability);
            await _repository.AddDonorAvailabilityAsync(entity);
            return DonorAvailabilityMapper.ToResponse(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var donorAvailability = await _repository.GetDonorAvailabilityByIdAsync(id);
            if (donorAvailability == null)
            {
                throw new Exception("Donor Availability not found");
            }

            await _repository.DeleteDonorAvailabilityAsync(donorAvailability);
        }

        public async Task<IEnumerable<DonorAvailabilityResponse>> GetAllAsync()
        {
            var donorAvailabilities = await _repository.GetAllDonorAvailabilitiesAsync();
            return donorAvailabilities.Select(DonorAvailabilityMapper.ToResponse);
        }

        public async Task<DonorAvailabilityResponse?> GetByIdAsync(int id)
        {
            var donorAvailability = await _repository.GetDonorAvailabilityByIdAsync(id);
            return donorAvailability == null ? null : DonorAvailabilityMapper.ToResponse(donorAvailability);
        }

        public async Task<DonorAvailabilityResponse?> UpdateAsync(int id, DonorAvailabilityRequest donorAvailability)
        {
            var existing = await _repository.GetDonorAvailabilityByIdAsync(id);
            if (existing == null)
                return null;

            existing.UserId = donorAvailability.UserId;
            existing.StatusDonorId = donorAvailability.StatusDonorId;
            existing.LastDonationDate = donorAvailability.LastDonationDate;
            existing.RecoveryReminderDate = donorAvailability.RecoveryReminderDate;
            existing.AvailableDate = donorAvailability.AvailableDate;

            var updated = await _repository.UpdateDonorAvailabilityAsync(existing);
            return DonorAvailabilityMapper.ToResponse(updated);
        }

        public async Task<IEnumerable<DonorAvailabilityResponse>> GetAllAvailableDonorsAsync()
        {
            var availableDonors = await _repository.GetAllAvailableDonorAsync();
            return availableDonors.Select(DonorAvailabilityMapper.ToResponse);
        }

        public async Task<IEnumerable<DonorAvailabilityResponse>> GetAvailableDonorsByBloodTypeAsync(string bloodType)
        {
            var availableDonors = await _repository.GetAvailableDonorsByBloodTypeAsync(bloodType);
            return availableDonors.Select(DonorAvailabilityMapper.ToResponse);
        }

        public async Task<IEnumerable<DonorAvailabilityResponse>> SearchCompatibleDonorsAsync(string recipientBloodType)
        {
            var compatibleDonors = await _repository.SearchCompatibleDonorsAsync(recipientBloodType);
            return compatibleDonors.Select(DonorAvailabilityMapper.ToResponse);
        }
    }
}
