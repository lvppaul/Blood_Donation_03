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
    public class DonorAvailabilityMapper
    {
        public static DonorAvailability ToEntity(DonorAvailabilityRequest request)
        {
            return new DonorAvailability
            {
                UserId = request.UserId,
                StatusDonorId = request.StatusDonorId,
                LastDonationDate = request.LastDonationDate,
                RecoveryReminderDate = request.RecoveryReminderDate,
                AvailableDate = request.AvailableDate,
                IsDeleted = false,
            };
        }

        public static DonorAvailabilityResponse ToResponse(DonorAvailability entity)
        {
            return new DonorAvailabilityResponse
            {
                AvailabilityId = entity.AvailabilityId,
                LastDonationDate = entity.LastDonationDate,
                RecoveryReminderDate = entity.RecoveryReminderDate,
                AvailableDate = entity.AvailableDate,
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
                StatusDonor = entity.StatusDonor != null ? new StatusBloodDonorResponse
                {
                    StatusDonorId = entity.StatusDonor.StatusDonorId,
                    StatusName = entity.StatusDonor.StatusName,
                    IsDeleted = entity.StatusDonor.IsDeleted
                } : null
            };
        }
    }
}
