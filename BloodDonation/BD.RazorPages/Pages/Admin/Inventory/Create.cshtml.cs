using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BD.Repositories.Models.Entities;
using BD.Services.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using System.ComponentModel.DataAnnotations;

namespace BD.RazorPages.Pages.Admin.Inventory
{
    public class CreateModel : PageModel
    {
       private readonly IBloodInventoryService _bloodInventoryService;
       private readonly IMedicalFacilityService _medicalFacilityService;
       private readonly IStatusBloodInventoryService _statusBloodInventoryService;

        public CreateModel(IBloodInventoryService bloodInventoryService, 
                          IMedicalFacilityService medicalFacilityService,
                          IStatusBloodInventoryService statusBloodInventoryService)
        {
            _bloodInventoryService = bloodInventoryService;
            _medicalFacilityService = medicalFacilityService;
            _statusBloodInventoryService = statusBloodInventoryService;
        }

        [BindProperty]
        public BloodInventoryRequest BloodInventory { get; set; } = new BloodInventoryRequest();

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

        public async Task OnGetAsync()
        {
            await LoadSelectListsAsync();
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

                // Validate the request object
                if (BloodInventory == null)
                {
                    TempData["ErrorMessage"] = "Invalid blood inventory data.";
                    await LoadSelectListsAsync();
                    return Page();
                }

                // Additional validation
                if (BloodInventory.ExpiredDate <= DateOnly.FromDateTime(DateTime.Now))
                {
                    ModelState.AddModelError("BloodInventory.ExpiredDate", "Expiry date must be in the future.");
                    await LoadSelectListsAsync();
                    return Page();
                }

                if (BloodInventory.Amount <= 0)
                {
                    ModelState.AddModelError("BloodInventory.Amount", "Amount must be greater than 0.");
                    await LoadSelectListsAsync();
                    return Page();
                }

                var result = await _bloodInventoryService.AddAsync(BloodInventory);
                
                if (result != null)
                {
                    TempData["SuccessMessage"] = $"Blood inventory created successfully! Added {BloodInventory.Amount} units of {BloodInventory.BloodType} {BloodInventory.ComponentType} to the inventory.";
                    return RedirectToPage("./BloodUnits");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create blood inventory. Please try again.";
                    await LoadSelectListsAsync();
                    return Page();
                }
            }
            catch (ArgumentNullException ex)
            {
                TempData["ErrorMessage"] = $"Missing required data: {ex.Message}";
                await LoadSelectListsAsync();
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating blood inventory: {ex.Message}";
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
                TempData["ErrorMessage"] = $"Error loading data: {ex.Message}";
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
    }
}
