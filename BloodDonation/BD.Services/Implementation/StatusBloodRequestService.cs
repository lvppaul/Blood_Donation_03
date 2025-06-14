using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Mappers;
using BD.Services.Interfaces;

namespace BD.Services.Implementation
{
    public class StatusBloodRequestService : IStatusBloodRequestService
    {
        private readonly IStatusBloodRequestRepository _repository;
        public StatusBloodRequestService(IStatusBloodRequestRepository repository)
        {
            _repository = repository;
        }
        public async Task<StatusBloodRequestResponse> AddAsync(StatusBloodRequestRequest statusRequestRequest)
        {
            var statusEntity = StatusBloodRequestMapper.ToEntity(statusRequestRequest);
            await _repository.AddAsync(statusEntity);
            return StatusBloodRequestMapper.ToResponse(statusEntity);
        }

        public async Task DeleteAsync(int id)
        {
            var request = await _repository.GetByIdAsync(id);
            if (request == null)
            {
                throw new Exception("Status Blood Request not found");
            }

            await _repository.DeleteAsync(request);
        }

        public async Task<IEnumerable<StatusBloodRequestResponse>> GetAllAsync()
        {
            var statusRequests = await _repository.GetAllAsync();
            return statusRequests.Select(StatusBloodRequestMapper.ToResponse);
        }

        public async Task<StatusBloodRequestResponse?> GetByIdAsync(int id)
        {
            var statusRequest = await _repository.GetByIdAsync(id);
            return statusRequest == null ? null : StatusBloodRequestMapper.ToResponse(statusRequest);
        }

        public async Task<StatusBloodRequestResponse?> UpdateAsync(int statusRequestId, StatusBloodRequestRequest statusRequestRequest)
        {
            var entity = await _repository.GetByIdAsync(statusRequestId);
            if (entity != null)
            {
                // Update properties
                entity.StatusName = statusRequestRequest.StatusName;
                await _repository.UpdateAsync(entity);
                return StatusBloodRequestMapper.ToResponse(entity);
            }
            return null;
        }
    }
}
