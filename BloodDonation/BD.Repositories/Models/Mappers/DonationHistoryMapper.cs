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
    public static class DonationHistoryMapper
    {
        public static DonationHistory ToEntity(DonationHistoryRequest donationHistoryRequest)
        {
            return new DonationHistory
            {
                DonationDate = DateTime.UtcNow,
                BloodType = donationHistoryRequest.BloodType,
                ComponentType = donationHistoryRequest.ComponentType,
                RequestId = donationHistoryRequest.RequestId,
                FacilityId = donationHistoryRequest.FacilityId,
                Amount = donationHistoryRequest.Amount,
                IsDeleted = false
            };
        }

        public static DonationHistoryResponse ToResponse(DonationHistory donationHistory)
        {
            return new DonationHistoryResponse
            {
                DonationId = donationHistory.DonationId,
                Status = donationHistory.Status,
                CreatedDate = donationHistory.CreatedDate,
                ConfirmedDate = donationHistory.ConfirmedDate,
                User = new UserResponse
                {
                    UserId = donationHistory.User.UserId,
                    Email = donationHistory.User.Email,
                    Address = donationHistory.User.Address,
                    BloodType = donationHistory.User.BloodType,
                    Name = donationHistory.User.Name,
                    CreatedAt = donationHistory.User.CreatedAt,
                    Phone = donationHistory.User.Phone,
                    Role = new RoleResponse
                    {
                        RoleId = donationHistory.User.RoleId,
                        Name = donationHistory.User.Role.Name
                    }
                },
                Facility = new MedicalFacilityResponse
                {
                    FacilityId = donationHistory.Facility.FacilityId,
                    Name = donationHistory.Facility.Name,
                    Address = donationHistory.Facility.Address,
                    Phone = donationHistory.Facility.Phone,
                    Email = donationHistory.Facility.Email,
                    Description = donationHistory.Facility.Description
                },
                Amount = donationHistory.Amount,
                DonationDate = donationHistory.DonationDate,
                BloodType = donationHistory.BloodType,
                ComponentType = donationHistory.ComponentType
            };
        }
    }
}
