using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Mappers;
using BD.Services.Interfaces;

namespace BD.Services.Implementation
{
    public class BloodCompatibilityService : IBloodCompatibilityService
    {
        private readonly IBloodCompatibilityRepository _repository;

        public BloodCompatibilityService(IBloodCompatibilityRepository repository)
        {
            _repository = repository;
        }
        public async Task<BloodCompatibilityResponse> AddAsync(BloodCompatibilityRequest request)
        {
            var entity = BloodCompatibilityMapper.ToEntity(request);
            await _repository.AddAsync(entity);
            return BloodCompatibilityMapper.ToResponse(entity);
        }

        public async Task DeleteAsync(int bloodCompatibilityId)
        {
            await _repository.DeleteAsync(bloodCompatibilityId);
        }

        public async Task<IEnumerable<BloodCompatibilityResponse>> GetAllAsync()
        {
            var bloodCompatibilities = await _repository.GetAllAsync();
            return bloodCompatibilities.Select(BloodCompatibilityMapper.ToResponse);
        }

        public async Task<BloodCompatibilityResponse?> GetByIdAsync(int bloodCompatibilityId)
        {
            var bloodCompatibility = await _repository.GetByIdAsync(bloodCompatibilityId);
            return bloodCompatibility == null ? null : BloodCompatibilityMapper.ToResponse(bloodCompatibility);
        }

        public async Task<BloodCompatibilityResponse?> UpdateAsync(int bloodCompatibilityId, BloodCompatibilityRequest request)
        {
            var bloodCompatibility = await _repository.GetByIdAsync(bloodCompatibilityId);
            if (bloodCompatibility != null)
            {
                bloodCompatibility.RecipientBloodType = request.RecipientBloodType;
                bloodCompatibility.DonorBloodType = request.DonorBloodType;
                bloodCompatibility.ComponentType = request.ComponentType;
                bloodCompatibility.IsCompatible = request.IsCompatible;
                await _repository.UpdateAsync(bloodCompatibility);
                return BloodCompatibilityMapper.ToResponse(bloodCompatibility);
            }
            return null;
        }
    }
}
