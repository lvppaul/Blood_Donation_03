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
    public class BloodRequestService : IBloodRequestService
    {
        private readonly IBloodRequestRepository _bloodRequestRepository;
        public BloodRequestService(IBloodRequestRepository bloodRequestRepository)
        {
            _bloodRequestRepository = bloodRequestRepository;
        }
        public async Task<BloodRequestResponse> AddAsync(BloodRequestRequest request)
        {
            var entity = BloodRequestMapper.ToEntity(request);
            var addedEntity = await _bloodRequestRepository.AddBloodRequestAsync(entity);

            return BloodRequestMapper.ToResponse(addedEntity);
        }

        public async Task DeleteAsync(int requestId)
        {
            var existingRequest = await _bloodRequestRepository.GetBloodRequestByIdAsync(requestId);
            if(existingRequest == null)
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
