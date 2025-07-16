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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BD.Services.Implementation
{
    public class MedicalFacilityService : IMedicalFacilityService
    {
        private readonly IMedicalFacilityRepository _repository;
        public MedicalFacilityService(IMedicalFacilityRepository repository)
        {
            _repository = repository;
        }
        public async Task<MedicalFacilityResponse> AddAsync(MedicalFacilityRequest request)
        {
            var entity = MedicalFacilityMapper.ToEntity(request);
            await _repository.AddFacilityAsync(entity);
            return MedicalFacilityMapper.ToResponse(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var facility = await _repository.GetFacilityByIdAsync(id);
            if (facility == null)
            {
                throw new Exception("Medical facility not found");
            }
            await _repository.DeleteFacilityAsync(facility);
        }

        public async Task<IEnumerable<MedicalFacilityResponse>> GetAllAsync()
        {
            var facilities = await _repository.GetAllFacilitiesAsync();
            return facilities.Select(MedicalFacilityMapper.ToResponse);
        }

        public async Task<MedicalFacilityResponse?> GetByIdAsync(int id)
        {
            var facility = await _repository.GetFacilityByIdAsync(id);
            
            return facility == null ? null : MedicalFacilityMapper.ToResponse(facility);
        }

        public async Task<MedicalFacilityResponse?> UpdateAsync(int id, MedicalFacilityRequest request)
        {
            var facilityFound = await _repository.GetFacilityByIdAsync(id);
            if (facilityFound != null)
            {
                facilityFound.Name = request.Name;
                facilityFound.Address = request.Address;
                facilityFound.Phone = request.Phone;
                facilityFound.Email = request.Email;
                facilityFound.Description = request.Description;
                var updatedFacility = await _repository.UpdateFacilityAsync(facilityFound);

                return MedicalFacilityMapper.ToResponse(updatedFacility);
            }
            return null;
        }
        public async Task<List<SelectListItem>> GetFacilitSelectListAsync()
        {
            try
            {
                var list = await _repository.GetAllFacilitiesAsync();

                if (list == null || !list.Any())
                {
                    return new List<SelectListItem>
                    {
                        new SelectListItem { Value = "", Text = "No facilities available" }
                    };
                }

                var selectList = list.Select(c => new SelectListItem
                {
                    Value = c.FacilityId.ToString(),
                    Text = c.Name ?? "Unnamed Facility"
                }).ToList();

                selectList.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Select a Facility"
                });

                return selectList;
            }
            catch (Exception ex)
            {
                // Log the error if you have logging
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Error loading facilities" }
                };
            }
        }
    }
}
