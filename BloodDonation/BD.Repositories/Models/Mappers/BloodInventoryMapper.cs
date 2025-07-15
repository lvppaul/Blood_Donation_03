using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.Mappers
{
    public class BloodInventoryMapper
    {
        public static BloodInventoryResponse ToResponse(BloodInventory entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "BloodInventory entity cannot be null");

            return new BloodInventoryResponse
            {
                InventoryId = entity.InventoryId,
                ComponentType = entity.ComponentType ?? string.Empty,
                Amount = entity.Amount,
                ExpiredDate = entity.ExpiredDate,
                LastUpdated = entity.LastUpdated,
                BloodType = entity.BloodType ?? string.Empty,
                StatusBloodInventory = entity.StatusInventory != null ? new StatusBloodInventoryResponse
                {
                    StatusInventoryId = entity.StatusInventory.StatusInventoryId,
                    StatusName = entity.StatusInventory.StatusName ?? "Unknown Status"
                } : new StatusBloodInventoryResponse
                {
                    StatusInventoryId = 0,
                    StatusName = "No Status"
                },
                Facility = entity.Facility != null ? new MedicalFacilityResponse
                {
                    FacilityId = entity.Facility.FacilityId,
                    Name = entity.Facility.Name ?? "Unknown Facility",
                    Address = entity.Facility.Address ?? string.Empty,
                    Phone = entity.Facility.Phone ?? string.Empty,
                    Email = entity.Facility.Email ?? string.Empty,
                    Description = entity.Facility.Description ?? string.Empty
                } : new MedicalFacilityResponse
                {
                    FacilityId = 0,
                    Name = "No Facility",
                    Address = string.Empty,
                    Phone = string.Empty,
                    Email = string.Empty,
                    Description = string.Empty
                }
            };
        }

        public static BloodInventory ToEntity(BloodInventoryRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "BloodInventoryRequest cannot be null");

            return new BloodInventory
            {
                FacilityId = request.FacilityId,
                ComponentType = request.ComponentType ?? string.Empty,
                Amount = request.Amount,
                ExpiredDate = request.ExpiredDate,
                StatusInventoryId = request.StatusInventoryId,
                BloodType = request.BloodType ?? string.Empty,
                LastUpdated = DateTime.UtcNow,
                IsDeleted = false
            };
        }
    }
}
