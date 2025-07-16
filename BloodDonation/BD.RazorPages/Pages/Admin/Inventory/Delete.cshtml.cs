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
    public class DeleteModel : PageModel
    {
        private readonly IBloodInventoryService _bloodInventoryService;

        public DeleteModel(IBloodInventoryService bloodInventoryService)
        {
            _bloodInventoryService = bloodInventoryService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Blood inventory ID is required.";
                return RedirectToPage("./BloodUnits");
            }

            try
            {
                // Get the blood unit details before disposing
                var bloodUnit = await _bloodInventoryService.GetByIdAsync(id.Value);
                if (bloodUnit == null)
                {
                    TempData["ErrorMessage"] = "Blood inventory not found.";
                    return RedirectToPage("./BloodUnits");
                }

                // Perform soft delete (dispose)
                await _bloodInventoryService.DeleteAsync(id.Value);

                TempData["SuccessMessage"] = $"Blood unit #{id} has been successfully disposed. Blood Type: {bloodUnit.BloodType}, Component: {bloodUnit.ComponentType}, Amount: {bloodUnit.Amount} units.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error disposing blood unit: {ex.Message}";
            }

            return RedirectToPage("./BloodUnits");
        }
    }
}
