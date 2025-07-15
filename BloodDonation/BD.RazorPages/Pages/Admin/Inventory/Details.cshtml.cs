using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BD.Repositories.Models.Entities;
using BD.Services.Interfaces;
using BD.Repositories.Models.DTOs.Responses;

namespace BD.RazorPages.Pages.Admin.Inventory
{
    public class DetailsModel : PageModel
    {
        private readonly IBloodInventoryService _bloodInventoryService;

        public DetailsModel(IBloodInventoryService bloodInventoryService)
        {
            _bloodInventoryService = bloodInventoryService;
        }

        public BloodInventoryResponse BloodInventory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Blood inventory ID is required.";
                return RedirectToPage("./BloodUnits");
            }

            try
            {
                var bloodInventory = await _bloodInventoryService.GetByIdAsync(id.Value);
                if (bloodInventory == null)
                {
                    TempData["ErrorMessage"] = "Blood inventory not found.";
                    return RedirectToPage("./BloodUnits");
                }

                BloodInventory = bloodInventory;
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error retrieving blood inventory: {ex.Message}";
                return RedirectToPage("./BloodUnits");
            }
        }

        // Helper method to get expiry status
        public string GetExpiryStatus()
        {
            if (BloodInventory?.ExpiredDate == null) return "Unknown";
            
            var daysUntilExpiry = (BloodInventory.ExpiredDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days;
            
            if (daysUntilExpiry < 0)
                return "Expired";
            else if (daysUntilExpiry <= 7)
                return "Expiring Soon";
            else if (daysUntilExpiry <= 30)
                return "Good";
            else
                return "Fresh";
        }

        // Helper method to get expiry status CSS class
        public string GetExpiryStatusClass()
        {
            var status = GetExpiryStatus();
            return status switch
            {
                "Expired" => "bg-red-100 text-red-800",
                "Expiring Soon" => "bg-yellow-100 text-yellow-800",
                "Good" => "bg-blue-100 text-blue-800",
                "Fresh" => "bg-green-100 text-green-800",
                _ => "bg-gray-100 text-gray-800"
            };
        }

        // Helper method to calculate days since last updated
        public string GetLastUpdatedInfo()
        {
            if (BloodInventory?.LastUpdated == null) return "Unknown";
            
            var daysSince = (DateTime.Now - BloodInventory.LastUpdated.Value).Days;
            
            return daysSince == 0 ? "Today" : 
                   daysSince == 1 ? "1 day ago" : 
                   $"{daysSince} days ago";
        }

        public async Task<IActionResult> OnPostDisposeAsync(int id)
        {
            try
            {
                // Get the blood unit to dispose
                var bloodUnit = await _bloodInventoryService.GetByIdAsync(id);
                if (bloodUnit == null)
                {
                    TempData["ErrorMessage"] = "Blood unit not found.";
                    return RedirectToPage("./BloodUnits");
                }

                // Perform soft delete (dispose)
                await _bloodInventoryService.DeleteAsync(id);

                TempData["SuccessMessage"] = $"Blood unit #{id} has been successfully disposed. Blood Type: {bloodUnit.BloodType}, Component: {bloodUnit.ComponentType}, Amount: {bloodUnit.Amount} units.";
                
                // Redirect to blood units list after successful disposal
                return RedirectToPage("./BloodUnits");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error disposing blood unit: {ex.Message}";
                return RedirectToPage("./BloodUnits");
            }
        }
    }
}
