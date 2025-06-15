using Azure.Core;
using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Mappers;
using BD.Services.Interfaces;

namespace BD.Services.Implementation
{
    public class BloodInventoryService : IBloodInventoryService
    {
        private readonly IBloodInventoryRepository _bloodInventoryRepository;

        public BloodInventoryService(IBloodInventoryRepository bloodInventoryRepository)
        {
            _bloodInventoryRepository = bloodInventoryRepository;
        }

        public async Task<BloodInventoryResponse> AddAsync(BloodInventoryRequest bloodInventory)
        {
            var bloodInventoryObject = BloodInventoryMapper.ToEntity(bloodInventory);
            var createBloodInventory = await _bloodInventoryRepository.AddBloodInventoryAsync(bloodInventoryObject);

            return BloodInventoryMapper.ToResponse(createBloodInventory);
        }

        public async Task DeleteAsync(int id)
        {
            var bloodInventory = await _bloodInventoryRepository.GetBloodInventoryByIdAsync(id);
            if (bloodInventory == null)
            {
                throw new Exception("Blood Inventory not found");
            }

            await _bloodInventoryRepository.DeleteBloodInventoryAsync(bloodInventory);
        }

        public async Task<IEnumerable<BloodInventoryResponse>> GetAllAsync()
        {
            var bloodInventories = await _bloodInventoryRepository.GetAllBloodInventoriesAsync();

            return bloodInventories.Select(bi => BloodInventoryMapper.ToResponse(bi));
        }

        public async Task<BloodInventoryResponse?> GetByIdAsync(int id)
        {
            var bloodInventory = await _bloodInventoryRepository.GetBloodInventoryByIdAsync(id);
            return bloodInventory == null ? null : BloodInventoryMapper.ToResponse(bloodInventory);
        }

        public async Task<BloodInventoryResponse?> UpdateAsync(int id, BloodInventoryRequest request)
        {
            var existingInventory = await _bloodInventoryRepository.GetBloodInventoryByIdAsync(id);
            if (existingInventory == null)
            {
                return null;
            }

            existingInventory.FacilityId = request.FacilityId;
            existingInventory.ComponentType = request.ComponentType;
            existingInventory.Amount = request.Amount;
            existingInventory.ExpiredDate = request.ExpiredDate;
            existingInventory.StatusInventoryId = request.StatusInventoryId;
            existingInventory.BloodType = request.BloodType;
            existingInventory.LastUpdated = DateTime.UtcNow;

            var updatedEntity = await _bloodInventoryRepository.UpdateBloodInventoryAsync(existingInventory);

            return BloodInventoryMapper.ToResponse(updatedEntity);
        }
    }
}
