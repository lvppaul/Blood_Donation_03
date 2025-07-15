using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BD.Services.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using System.ComponentModel.DataAnnotations;

namespace BD.RazorPages.Pages.Admin.Inventory
{
    public class EditModel : PageModel
    {
        private readonly IBloodInventoryService _bloodInventoryService;
        private readonly IMedicalFacilityService _medicalFacilityService;
        private readonly IStatusBloodInventoryService _statusBloodInventoryService;

        public EditModel(
            IBloodInventoryService bloodInventoryService,
            IMedicalFacilityService medicalFacilityService,
            IStatusBloodInventoryService statusBloodInventoryService)
        {
            _bloodInventoryService = bloodInventoryService;
            _medicalFacilityService = medicalFacilityService;
            _statusBloodInventoryService = statusBloodInventoryService;
        }

        [BindProperty]
        public int InventoryId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Medical facility is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid facility")]
        public int FacilityId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Component type is required")]
        [StringLength(50, ErrorMessage = "Component type cannot exceed 50 characters")]
        public string ComponentType { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Amount is required")]
        [Range(1, 10000, ErrorMessage = "Amount must be between 1 and 10,000 units")]
        public int Amount { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Expiry date is required")]
        public DateOnly ExpiredDate { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Status is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid status")]
        public int StatusInventoryId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Blood type is required")]
        [RegularExpression(@"^(A|B|AB|O)[+-]$", ErrorMessage = "Please enter a valid blood type (e.g., A+, B-, O+, AB-)")]
        public string BloodType { get; set; } = string.Empty;

        // Properties for dropdowns
        public List<SelectListItem> MedicalFacilities { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> StatusBloodInventories { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> BloodTypes { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Select Blood Type" },
            new SelectListItem { Value = "A+", Text = "A+" },
            new SelectListItem { Value = "A-", Text = "A-" },
            new SelectListItem { Value = "B+", Text = "B+" },
            new SelectListItem { Value = "B-", Text = "B-" },
            new SelectListItem { Value = "AB+", Text = "AB+" },
            new SelectListItem { Value = "AB-", Text = "AB-" },
            new SelectListItem { Value = "O+", Text = "O+" },
            new SelectListItem { Value = "O-", Text = "O-" }
        };

        public List<SelectListItem> ComponentTypes { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Select Component Type" },
            new SelectListItem { Value = "Whole Blood", Text = "Whole Blood" },
            new SelectListItem { Value = "Red Blood Cells", Text = "Red Blood Cells" },
            new SelectListItem { Value = "Plasma", Text = "Plasma" },
            new SelectListItem { Value = "Platelets", Text = "Platelets" },
            new SelectListItem { Value = "White Blood Cells", Text = "White Blood Cells" }
        };

        // Original values for comparison
        public BloodInventoryResponse OriginalInventory { get; set; } = new BloodInventoryResponse();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Blood inventory ID is required.";
                return RedirectToPage("./BloodUnits");
            }

            try
            {
                await LoadSelectListsAsync();
                
                var bloodInventory = await _bloodInventoryService.GetByIdAsync(id.Value);
                if (bloodInventory == null)
                {
                    TempData["ErrorMessage"] = "Blood inventory not found.";
                    return RedirectToPage("./BloodUnits");
                }

                // Populate form properties
                InventoryId = bloodInventory.InventoryId;
                FacilityId = bloodInventory.Facility.FacilityId;
                ComponentType = bloodInventory.ComponentType;
                Amount = bloodInventory.Amount;
                ExpiredDate = bloodInventory.ExpiredDate;
                StatusInventoryId = bloodInventory.StatusBloodInventory.StatusInventoryId;
                BloodType = bloodInventory.BloodType;

                // Store original values for comparison
                OriginalInventory = bloodInventory;

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading blood inventory: {ex.Message}";
                return RedirectToPage("./BloodUnits");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadSelectListsAsync();
                    return Page();
                }

                // Additional business validation
                if (ExpiredDate <= DateOnly.FromDateTime(DateTime.Now))
                {
                    ModelState.AddModelError("ExpiredDate", "Expiry date must be in the future.");
                    await LoadSelectListsAsync();
                    return Page();
                }

                // Create update request
                var updateRequest = new BloodInventoryRequest
                {
                    FacilityId = FacilityId,
                    ComponentType = ComponentType,
                    Amount = Amount,
                    ExpiredDate = ExpiredDate,
                    StatusInventoryId = StatusInventoryId,
                    BloodType = BloodType
                };

                var result = await _bloodInventoryService.UpdateAsync(InventoryId, updateRequest);
                
                if (result != null)
                {
                    TempData["SuccessMessage"] = $"Blood inventory #{InventoryId} updated successfully! Updated {Amount} units of {BloodType} {ComponentType}.";
                    return RedirectToPage("./BloodUnits");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update blood inventory. Please try again.";
                    await LoadSelectListsAsync();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating blood inventory: {ex.Message}";
                await LoadSelectListsAsync();
                return Page();
            }
        }

        private async Task LoadSelectListsAsync()
        {
            try
            {
                // Load medical facilities
                var facilities = await _medicalFacilityService.GetAllAsync();
                MedicalFacilities = facilities?.Select(f => new SelectListItem
                {
                    Value = f.FacilityId.ToString(),
                    Text = f.Name
                }).ToList() ?? new List<SelectListItem>();
                
                if (!MedicalFacilities.Any())
                {
                    MedicalFacilities.Add(new SelectListItem { Value = "", Text = "No facilities available" });
                }
                else
                {
                    MedicalFacilities.Insert(0, new SelectListItem { Value = "", Text = "Select a Facility" });
                }

                // Load status blood inventories
                var statuses = await _statusBloodInventoryService.GetAllAsync();
                StatusBloodInventories = statuses?.Select(s => new SelectListItem
                {
                    Value = s.StatusInventoryId.ToString(),
                    Text = s.StatusName
                }).ToList() ?? new List<SelectListItem>();
                
                if (!StatusBloodInventories.Any())
                {
                    StatusBloodInventories.Add(new SelectListItem { Value = "", Text = "No statuses available" });
                }
                else
                {
                    StatusBloodInventories.Insert(0, new SelectListItem { Value = "", Text = "Select Status" });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading form data: {ex.Message}";
                MedicalFacilities = new List<SelectListItem> 
                { 
                    new SelectListItem { Value = "", Text = "Error loading facilities" } 
                };
                StatusBloodInventories = new List<SelectListItem> 
                { 
                    new SelectListItem { Value = "", Text = "Error loading statuses" } 
                };
            }
        }

        // Helper method to get changes summary
        public List<string> GetChangesSummary()
        {
            var changes = new List<string>();
            
            if (OriginalInventory != null)
            {
                if (FacilityId != OriginalInventory.Facility.FacilityId)
                    changes.Add("Facility changed");
                if (ComponentType != OriginalInventory.ComponentType)
                    changes.Add($"Component type: {OriginalInventory.ComponentType} → {ComponentType}");
                if (Amount != OriginalInventory.Amount)
                    changes.Add($"Amount: {OriginalInventory.Amount} → {Amount} units");
                if (ExpiredDate != OriginalInventory.ExpiredDate)
                    changes.Add($"Expiry date: {OriginalInventory.ExpiredDate:MMM dd, yyyy} → {ExpiredDate:MMM dd, yyyy}");
                if (StatusInventoryId != OriginalInventory.StatusBloodInventory.StatusInventoryId)
                    changes.Add("Status changed");
                if (BloodType != OriginalInventory.BloodType)
                    changes.Add($"Blood type: {OriginalInventory.BloodType} → {BloodType}");
            }
            
            return changes;
        }
    }
}
