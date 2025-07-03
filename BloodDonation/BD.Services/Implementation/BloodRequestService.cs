using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Mappers;
using BD.Services.Interfaces;

namespace BD.Services.Implementation
{
    public class BloodRequestService : IBloodRequestService
    {
        private readonly IBloodRequestRepository _bloodRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStatusBloodRequestRepository _statusRequestRepository;
        public BloodRequestService(IBloodRequestRepository bloodRequestRepository, IUserRepository userRepository, IStatusBloodRequestRepository statusBloodRequestRepository)
        {
            _bloodRequestRepository = bloodRequestRepository;
            _userRepository = userRepository;
            _statusRequestRepository = statusBloodRequestRepository;
        }
        public async Task<BloodRequestResponse> AddAsync(BloodRequestRequest request)
        {
            var entity = BloodRequestMapper.ToEntity(request);

            var userEntity = await _userRepository.GetByIdAsync(request.UserId);
            if (userEntity == null)
            {
                throw new Exception("User not found");
            }
            var statusEntity = await _statusRequestRepository.GetByIdAsync(request.StatusRequestId);
            if (statusEntity == null)
            {
                throw new Exception("Status request not found");
            }
            entity.User = userEntity;
            entity.StatusRequest = statusEntity;
            var addedEntity = await _bloodRequestRepository.AddBloodRequestAsync(entity);
            return BloodRequestMapper.ToResponse(addedEntity);
        }

        public async Task DeleteAsync(int requestId)
        {
            var existingRequest = await _bloodRequestRepository.GetBloodRequestByIdAsync(requestId);
            if (existingRequest == null)
            {
                throw new Exception("Blood request not found");
            }

            await _bloodRequestRepository.DeleteBloodRequestAsync(existingRequest);
        }

        public async Task<IEnumerable<BloodRequestResponse>> GetAllAsync()
        {
            var bloodRequests = await _bloodRequestRepository.GetAllBloodRequestsAsync();

            return bloodRequests.Select(br => BloodRequestMapper.ToResponse(br));
        }

        public async Task<BloodRequestResponse?> GetByIdAsync(int requestId)
        {
            var bloodRequest = await _bloodRequestRepository.GetBloodRequestByIdAsync(requestId);
            if (bloodRequest == null)
            {
                return null;
            }
            return BloodRequestMapper.ToResponse(bloodRequest);
        }

        public async Task<BloodRequestResponse?> UpdateAsync(int id, BloodRequestRequest request)
        {
            var existingRequest = await _bloodRequestRepository.GetBloodRequestByIdAsync(id);
            if (existingRequest == null)
            {
                return null;
            }

            existingRequest.UserId = request.UserId;
            existingRequest.BloodType = request.BloodType;
            existingRequest.ComponentType = request.ComponentType;
            existingRequest.Amount = request.Amount;
            existingRequest.UrgencyLevel = request.UrgencyLevel;
            existingRequest.RequestDate = request.RequestDate;
            existingRequest.StatusRequestId = request.StatusRequestId;
            existingRequest.FulfilledDate = request.FulfilledDate;

            var updated = await _bloodRequestRepository.UpdateBloodRequestAsync(existingRequest);

            return BloodRequestMapper.ToResponse(updated);
        }
    }
}
