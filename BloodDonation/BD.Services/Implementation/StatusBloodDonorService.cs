using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Mappers;
using BD.Services.Interfaces;

namespace BD.Services.Implementation
{
    public class StatusBloodDonorService : IStatusBloodDonorService
    {
        private readonly IStatusBloodDonorRepository _repository;

        public StatusBloodDonorService(IStatusBloodDonorRepository repository)
        {
            _repository = repository;
        }

        public async Task<StatusBloodDonorResponse> AddAsync(StatusBloodDonorRequest request)
        {
            var statusEntity = StatusBloodDonorMapper.ToEntity(request);
            await _repository.AddAsync(statusEntity);
            return StatusBloodDonorMapper.ToResponseDto(statusEntity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<StatusBloodDonorResponse>> GetAllAsync()
        {
            var statusDonors = await _repository.GetAllAsync();
            return statusDonors.Select(StatusBloodDonorMapper.ToResponseDto);
        }

        public async Task<StatusBloodDonorResponse?> GetByIdAsync(int id)
        {
            var statusDonors = await _repository.GetByIdAsync(id);
            return statusDonors == null ? null : StatusBloodDonorMapper.ToResponseDto(statusDonors);
        }

        public async Task<StatusBloodDonorResponse?> UpdateAsync(int statusDonorId, StatusNotificationRequest statusDonorRequest)
        {
            var entity = await _repository.GetByIdAsync(statusDonorId);
            if (entity != null)
            {
                // Update properties
                entity.StatusName = statusDonorRequest.StatusName;
                await _repository.UpdateAsync(entity);
                return StatusBloodDonorMapper.ToResponseDto(entity);
            }
            return null;
        }


    }
}
