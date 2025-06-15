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
    public class BloodRequestMapper
    {
        public static BloodRequest ToEntity(BloodRequestRequest request)
        {
            return new BloodRequest
            {
                UserId = request.UserId,
                BloodType = request.BloodType,
                ComponentType = request.ComponentType,
                Amount = request.Amount,
                UrgencyLevel = request.UrgencyLevel,
                RequestDate = request.RequestDate,
                StatusRequestId = request.StatusRequestId,
                FulfilledDate = request.FulfilledDate,
                IsDeleted = false
            };
        }

        public static BloodRequestResponse ToResponse(BloodRequest entity)
        {
            return new BloodRequestResponse
            {
                RequestId = entity.RequestId,
                BloodType = entity.BloodType,
                ComponentType = entity.ComponentType,
                Amount = entity.Amount,
                UrgencyLevel = entity.UrgencyLevel,
                RequestDate = entity.RequestDate,
                FulfilledDate = entity.FulfilledDate,
                IsDeleted = entity.IsDeleted,
                User = new UserResponse
                {
                    UserId = entity.User.UserId,
                    Name = entity.User.Name,
                    BloodType = entity.User.BloodType,
                    Phone = entity.User.Phone,
                    Email = entity.User.Email,
                    Address = entity.User.Address,
                    CreatedAt = entity.User.CreatedAt,
                    IsDeleted = entity.User.IsDeleted,
                    Role = new RoleResponse
                    {
                        RoleId = entity.User.Role.RoleId,
                        Name = entity.User.Role.Name
                    }
                },

                StatusBloodRequestResponse = new StatusBloodRequestResponse
                {
                    StatusRequestId = entity.StatusRequest.StatusRequestId,
                    StatusName = entity.StatusRequest.StatusName
                }
            };
        }
    }
}
