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
            return new BloodInventoryResponse
            {
                InventoryId = entity.InventoryId,
                ComponentType = entity.ComponentType,
                Amount = entity.Amount,
                ExpiredDate = entity.ExpiredDate,
                LastUpdated = entity.LastUpdated,
                BloodType = entity.BloodType,
                StatusBloodInventory = new StatusBloodInventoryResponse
                {
                    StatusInventoryId = entity.StatusInventory.StatusInventoryId,
                    StatusName = entity.StatusInventory.StatusName
                },
                Facility = new MedicalFacilityResponse
                {
                    FacilityId = entity.Facility.FacilityId,
                    Name = entity.Facility.Name,
                    Address = entity.Facility.Address,
                    Phone = entity.Facility.Phone,
                    Email = entity.Facility.Email,
                    Description = entity.Facility.Description
                }
            };
        }

        public static BloodInventory ToEntity(BloodInventoryRequest request)
        {
            return new BloodInventory
            {
                FacilityId = request.FacilityId,
                ComponentType = request.ComponentType,
                Amount = request.Amount,
                ExpiredDate = request.ExpiredDate,
                StatusInventoryId = request.StatusInventoryId,
                BloodType = request.BloodType,
                LastUpdated = DateTime.UtcNow
            };
        }
    }
}
