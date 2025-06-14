using BD.Repositories.Implementation;
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
                throw new Exception("Blood Inventory post not found");
            }

            await _bloodInventoryRepository.DeleteBloodInventoryAsync(bloodInventory);
        }

        public Task<IEnumerable<BloodInventoryResponse>> GetAllPostsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BloodInventoryResponse?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BloodInventoryResponse?> UpdateAsync(BloodInventoryRequest post)
        {
            throw new NotImplementedException();
        }
    }
}
