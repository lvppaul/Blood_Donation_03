using BD.Repositories.Models.DTOs.Requests;
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

        public static DonationHistoryRequest ToRequest(DonationHistory donationHistory)
        {
            return new DonationHistoryRequest
            {
                RequestId = donationHistory.RequestId,
                FacilityId = donationHistory.FacilityId,
                Amount = donationHistory.Amount,
                DonationDate = donationHistory.DonationDate,
                BloodType = donationHistory.BloodType,
                ComponentType = donationHistory.ComponentType
            };
        }
    }
}
