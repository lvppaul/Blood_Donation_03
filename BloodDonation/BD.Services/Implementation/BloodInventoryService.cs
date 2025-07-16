﻿using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Mappers;
using BD.Services.Enum;
using BD.Services.Interfaces;
using BD.Services.Others;

namespace BD.Services.Implementation
{
    public class BloodInventoryService : IBloodInventoryService
    {
        private readonly IBloodInventoryRepository _bloodInventoryRepository;
        private readonly IMedicalFacilityRepository _medicalFacilityRepository;
        private readonly IStatusBloodInventoryRepository _statusInventoryRepository;

        public BloodInventoryService(IBloodInventoryRepository bloodInventoryRepository, IMedicalFacilityRepository medicalFacilityRepository, IStatusBloodInventoryRepository statusInventoryRepository)
        {
            _bloodInventoryRepository = bloodInventoryRepository;
            _medicalFacilityRepository = medicalFacilityRepository;
            _statusInventoryRepository = statusInventoryRepository;
        }

        public async Task<BloodInventoryResponse> AddAsync(BloodInventoryRequest bloodInventory)
        {
            var bloodInventoryObject = BloodInventoryMapper.ToEntity(bloodInventory);
            var facilityEntity = await _medicalFacilityRepository.GetFacilityByIdAsync(bloodInventory.FacilityId);
            if (facilityEntity == null)
            {
                throw new Exception("Facility not found");
            }
            var statusEntity = await _statusInventoryRepository.GetByIdAsync(bloodInventory.StatusInventoryId);
            if (statusEntity == null)
            {
                throw new Exception("Status inventory not found");
            }
            bloodInventoryObject.Facility = facilityEntity;
            bloodInventoryObject.StatusInventory = statusEntity;

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

            var facilityEntity = await _medicalFacilityRepository.GetFacilityByIdAsync(updatedEntity.FacilityId);
            if (facilityEntity == null)
            {
                throw new Exception("Facility not found");
            }
            var statusEntity = await _statusInventoryRepository.GetByIdAsync(updatedEntity.StatusInventoryId);
            if (statusEntity == null)
            {
                throw new Exception("Status inventory not found");
            }
            updatedEntity.Facility = facilityEntity;
            updatedEntity.StatusInventory = statusEntity;

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
        public async Task<(IEnumerable<BloodInventoryResponse>, int TotalCount)> GetFilteredAsync(
    string searchTerm = null,
    string bloodType = null,
    int? facilityId = null,
    int pageNumber = 1,
    int pageSize = 10)
        {
            var (bloodInventories, totalCount) = await _bloodInventoryRepository.GetFilteredBloodInventoriesAsync(
                searchTerm, bloodType, facilityId, pageNumber, pageSize);

            var response = bloodInventories.Select(bi => BloodInventoryMapper.ToResponse(bi));
            return (response, totalCount);
        }
    }
}
