using BD.Repositories.Models.DTOs.Requests;
using BD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BD.RazorPages.Pages
{
    public class EmergencyBloodRequestModel : PageModel
    {
        private readonly IBloodRequestService _bloodRequestService;
        private readonly IMedicalFacilityService _medicalService;
        private readonly ILogger<EmergencyBloodRequestModel> _logger;

        public EmergencyBloodRequestModel(IBloodRequestService bloodRequestService, ILogger<EmergencyBloodRequestModel> logger, IMedicalFacilityService medicalService)
        {
            _bloodRequestService = bloodRequestService;
            _logger = logger;
            _medicalService = medicalService;
        }

        [BindProperty]
        public BloodRequestRequest BloodRequest { get; set; } = new();

        // Additional properties for emergency-specific fields
        /*  [BindProperty]
          [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
          [Display(Name = "Số điện thoại liên hệ")]
          public string? ContactPhone { get; set; }

            [BindProperty]
            [Display(Name = "Tên bệnh viện")]
            [StringLength(200, ErrorMessage = "Tên bệnh viện không được vượt quá 200 ký tự")]
            public string? HospitalName { get; set; }

            [BindProperty]
            public List<SelectListItem> MedicalFacilities { get; set; } = new();*/


        [BindProperty]
        [Display(Name = "Note")]
        [StringLength(1000, ErrorMessage = "Note not exceed 1000 characters")]
        public string? Notes { get; set; }

        public void OnGet()
        {
            // Initialize form for emergency request
            BloodRequest = new BloodRequestRequest
            {
                UrgencyLevel = "Emergency",
                RequestDate = DateTime.Now,
                StatusRequestId = 1 // Pending status
            };
            // LoadMedicalFacilities();
        }
        /*   private async void LoadMedicalFacilities()
           {
               // Giả sử bạn có service/repository lấy danh sách MedicalFacility
               var facilities = await _medicalService.GetAllAsync(); // hoặc await nếu async
               MedicalFacilities = facilities
                   .Select(f => new SelectListItem { Value = f.FacilityId.ToString(), Text = f.Name })
                   .ToList();
           }*/

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // Lấy UserId từ claims nếu user đã đăng nhập, nếu không thì set = 1 (anonymous/guest)
                var userIdClaim = HttpContext.Session.GetString("UserId");
                BloodRequest.UserId = int.TryParse(userIdClaim, out int userId) ? userId : 3; // Default user if not logged in

                // Set các giá trị mặc định cho emergency request
                BloodRequest.UrgencyLevel = "Emergency";
                BloodRequest.RequestDate = DateTime.Now;
                BloodRequest.FulfilledDate = DateTime.Now.AddMonths(3);
                BloodRequest.StatusRequestId = 1; // Assuming 1 is Pending status

                // Log thông tin request
                _logger.LogInformation("Creating emergency blood request for user {UserId}, BloodType: {BloodType}, Amount: {Amount}, ComponentType: {ComponentType}",
                    BloodRequest.UserId, BloodRequest.BloodType, BloodRequest.Amount, BloodRequest.ComponentType);

                // Gửi request thông qua service
                var result = await _bloodRequestService.AddAsync(BloodRequest);

                if (result != null)
                {
                    TempData["SuccessMessage"] = $"Emergency Request Created Successfuly! ID: {result.RequestId}. We will contact you as soon as possible.";
                    _logger.LogInformation("Emergency blood request created successfully with ID: {RequestId}", result.RequestId);

                    // Reset form sau khi gửi thành công
                    BloodRequest = new BloodRequestRequest
                    {
                        UrgencyLevel = "Emergency",
                        RequestDate = DateTime.Now,
                        StatusRequestId = 1
                    };
                    // ContactPhone = "";
                    // HospitalName = "";
                    Notes = "";
                    return Page();
                }
                else
                {
                    TempData["ErrorMessage"] = "Error happens. Try again later.";
                    _logger.LogWarning("Failed to create emergency blood request - service returned null");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating emergency blood request for user {UserId}",
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous");
                TempData["ErrorMessage"] = "Error happens when create request. Try again later.";
                return Page();
            }
        }
    }
}
