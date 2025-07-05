using Azure.Core;
using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;
using BD.Repositories.Models.Mappers;
using BD.Services.Enum;
using BD.Services.Interfaces;
using BD.Services.Others;

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
        public async Task<IEnumerable<BloodInventoryResponse>> GetAllBloodInventoriesByBloodTypeAsync(string type)
        {
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    throw new ArgumentException("Blood type cannot be null or empty", nameof(type));
                }
                var bloodType = type.ToUpper();
                if (!BloodTypeEnum.GetAllBloodTypes().Contains(bloodType))
                {
                    throw new ArgumentException("Invalid blood type", nameof(type));
                }
                var bloodInventories = await _bloodInventoryRepository.GetAllBloodInventoriesByBloodTypeAsync(bloodType);
                return bloodInventories.Select(bi => BloodInventoryMapper.ToResponse(bi));
            }
            catch (Exception)
            {

                throw new Exception("Error at GetAllBloodInventoriesByBloodTypeAsync at BloodInventoryService");
            }

        }
        public async Task<int> TotalBloodInventoriesByBloodTypeAsync(string type)
        {
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    throw new ArgumentException("Blood type cannot be null or empty", nameof(type));
                }
                var bloodType = type.ToUpper();
                if (!BloodTypeEnum.GetAllBloodTypes().Contains(bloodType))
                {
                    throw new ArgumentException("Invalid blood type", nameof(type));
                }
                var bloodInventories = await _bloodInventoryRepository.GetAllBloodInventoriesByBloodTypeAsync(bloodType);
                if (bloodInventories == null)
                {
                    return 0;
                }

                // Calculate total amount instead of counting items
                return bloodInventories.Sum(bi => bi.Amount);
            }
            catch (Exception)
            {
                throw new Exception("Error at TotalBloodInventoriesByBloodTypeAsync at BloodInventoryService");
            }
        }

 
        public async Task<IEnumerable<BloodTypeOverview>> GetBloodTypeOverviewAsync()
        {
            try
            {
                var bloodTypeOverview = new List<BloodTypeOverview>();
                var allBloodTypes = BloodTypeEnum.GetAllBloodTypes();

                foreach (var bloodType in allBloodTypes)
                {
                    var volume = await TotalBloodInventoriesByBloodTypeAsync(bloodType);
                    bloodTypeOverview.Add(new BloodTypeOverview
                    {
                        BloodType = bloodType,
                        Volume = volume
                    });
                }

                return bloodTypeOverview;
            }
            catch (Exception ex)
            {
                throw new Exception("Error at GetBloodTypeOverviewAsync at BloodInventoryService ", ex);
            }   
           
        }
    }
}
