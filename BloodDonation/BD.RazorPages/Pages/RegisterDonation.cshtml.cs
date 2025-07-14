using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BD.RazorPages.Pages
{
    public class RegisterDonationModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IDonationHistoryRepository _donationHistoryRepository;
        private readonly IDonorAvailabilityRepository _donorAvailabilityRepository;
        private readonly ILogger<RegisterDonationModel> _logger;

        public RegisterDonationModel(
            IUserRepository userRepository,
            IDonationHistoryRepository donationHistoryRepository,
            IDonorAvailabilityRepository donorAvailabilityRepository,
            ILogger<RegisterDonationModel> logger)
        {
            _userRepository = userRepository;
            _donationHistoryRepository = donationHistoryRepository;
            _donorAvailabilityRepository = donorAvailabilityRepository;
            _logger = logger;
        }

        public string SuccessMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;

        // User Info (Read-only)
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public string UserBloodType { get; set; } = string.Empty;
        public string LastDonationDateDisplay { get; set; } = "No previous donations";

        // Form Properties with Validation
        [BindProperty]
        [Required(ErrorMessage = "Available date is required")]
        [DataType(DataType.Date)]
        public DateTime AvailableDate { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Component type is required")]
        public string ComponentType { get; set; } = "Whole Blood";

        [BindProperty]
        public DateTime RecoveryReminderDate { get; set; } // Auto-calculated

        [BindProperty]
        [StringLength(1000, ErrorMessage = "Additional notes cannot exceed 1000 characters")]
        public string? AdditionalNotes { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "You must agree to the terms and conditions")]
        public bool AgreesToTerms { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Login");
            }

            // Check if user has member role
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "member")
            {
                ErrorMessage = "Only members can register for blood donation. Please contact an administrator if you need access.";
                return RedirectToPage("/Profile");
            }

            // Load user data and last donation information
            await LoadUserDataAsync(userId);

            // Set default available date to tomorrow
            AvailableDate = DateTime.Today.AddDays(1);
            RecoveryReminderDate = CalculateRecoveryReminderDate(AvailableDate);
            ComponentType = "Whole Blood"; // Set default component type

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Check if user is logged in and has member role
            var userId = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Login");
            }

            if (userRole != "member")
            {
                ErrorMessage = "Only members can register for blood donation.";
                return RedirectToPage("/Profile");
            }

            // Auto-calculate recovery reminder date
            RecoveryReminderDate = CalculateRecoveryReminderDate(AvailableDate);

            // Custom validation
            if (AvailableDate <= DateTime.Now.Date)
            {
                ModelState.AddModelError(nameof(AvailableDate), "Available date must be in the future.");
            }

            if (!AgreesToTerms)
            {
                ModelState.AddModelError(nameof(AgreesToTerms), "You must agree to the terms and conditions.");
            }

            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please correct the errors below.";
                // Reload user data
                await LoadUserDataAsync(userId);
                return Page();
            }

            try
            {
                // Create donor availability record
                await CreateDonorAvailabilityAsync(userId);

                SuccessMessage = $"Your blood donation registration has been completed successfully! " +
                    $"Component: {ComponentType} | Available Date: {AvailableDate:MMM dd, yyyy} | " +
                    $"Your donation request is now in the system with 'Waiting' status. Our team will contact you when needed.";
                _logger.LogInformation("Donor availability and donation history registered for user {UserId}", userId);

                // Clear form data after successful submission
                ClearFormData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering donor availability for user {UserId}", userId);
                ErrorMessage = "An error occurred while registering your availability. Please try again.";
                // Reload user data on error
                await LoadUserDataAsync(userId);
            }

            return Page();
        }

        private async Task LoadUserDataAsync(string userId)
        {
            var userName = HttpContext.Session.GetString("UserName") ?? "";

            // For demo accounts, use session data
            if (userId.StartsWith("demo-"))
            {
                UserName = userName;
                UserEmail = GetDemoEmail(userId);
                UserPhone = GetDemoPhone(userId);
                UserBloodType = GetDemoBloodType(userId);
                LastDonationDateDisplay = "Demo - Last donated 3 months ago";
            }
            else
            {
                // Load real user data from database
                if (int.TryParse(userId, out int userIdInt))
                {
                    try
                    {
                        var user = await _userRepository.GetByIdAsync(userIdInt);
                        if (user != null)
                        {
                            UserName = user.Name;
                            UserEmail = user.Email ?? "";
                            UserPhone = user.Phone ?? "";
                            UserBloodType = user.BloodType ?? "";
                        }

                        // Get last donation date from donation history
                        var lastDonation = await _donationHistoryRepository.GetLatestDonationByUserIdAsync(userIdInt);
                        if (lastDonation != null && lastDonation.DonationDate.HasValue)
                        {
                            LastDonationDateDisplay = lastDonation.DonationDate.Value.ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            LastDonationDateDisplay = "No previous donations";
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error loading user data for {UserId}", userIdInt);
                        // Fall back to session data
                        UserName = userName;
                        UserEmail = "";
                        UserPhone = "";
                        UserBloodType = "";
                        LastDonationDateDisplay = "Unable to load donation history";
                    }
                }
            }
        }

        private async Task CreateDonorAvailabilityAsync(string userId)
        {
            // For demo accounts, just log the registration
            if (userId.StartsWith("demo-"))
            {
                _logger.LogInformation("Demo donor availability: {UserName}, Available: {AvailableDate}, Recovery: {RecoveryDate}",
                    UserName, AvailableDate, RecoveryReminderDate);
                return;
            }

            // For real users, create both donor availability and donation history records
            if (int.TryParse(userId, out int userIdInt))
            {
                // Create new donor availability record
                var donorAvailability = new DonorAvailability
                {
                    UserId = userIdInt,
                    StatusDonorId = 1, // Always set to 1 (Available status)
                    AvailableDate = DateOnly.FromDateTime(AvailableDate),
                    RecoveryReminderDate = DateOnly.FromDateTime(RecoveryReminderDate),
                    IsDeleted = false
                };

                // Set LastDonationDate if we have donation history
                var lastDonation = await _donationHistoryRepository.GetLatestDonationByUserIdAsync(userIdInt);
                if (lastDonation != null && lastDonation.DonationDate.HasValue)
                {
                    donorAvailability.LastDonationDate = DateOnly.FromDateTime(lastDonation.DonationDate.Value);
                }

                await _donorAvailabilityRepository.AddDonorAvailabilityAsync(donorAvailability);

                // Create donation history record with "Waiting" status
                try
                {
                    // Get user data to populate blood type
                    var user = await _userRepository.GetByIdAsync(userIdInt);
                    var userBloodType = user?.BloodType ?? "Unknown";

                    var donationHistory = new DonationHistory
                    {
                        UserId = userIdInt,
                        RequestId = null, // Default value - can be updated later when linked to a specific request
                        FacilityId = 1, // Default facility ID - can be configured or made configurable
                        Amount = 0, // Will be set when donation is confirmed
                        BloodType = userBloodType, // Use actual user blood type
                        ComponentType = ComponentType ?? "Whole Blood", // Use selected component type
                        Status = DonationStatus.Waiting, // Set initial status to Waiting
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    };

                    await _donationHistoryRepository.AddDonationHistoryAsync(donationHistory);
                    _logger.LogInformation("Successfully created donation history for user {UserId} with blood type {BloodType}", userIdInt, userBloodType);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating donation history for user {UserId}", userIdInt);
                    // Continue execution - don't fail the entire registration if only donation history fails
                }

                _logger.LogInformation("Created donor availability and donation history for user {UserId}", userIdInt);
            }
        }

        private DateTime CalculateRecoveryReminderDate(DateTime availableDate)
        {
            // Add 3 months (90 days) to the available date for recovery reminder
            return availableDate.AddDays(90);
        }

        private void ClearFormData()
        {
            // Clear form fields after successful submission
            AvailableDate = DateTime.Today.AddDays(1);
            RecoveryReminderDate = CalculateRecoveryReminderDate(AvailableDate);
            ComponentType = "Whole Blood";
            AdditionalNotes = string.Empty;
            AgreesToTerms = false;
        }

        private string GetDemoEmail(string userId)
        {
            return userId switch
            {
                "demo-member" => "member@bloodconnect.com",
                _ => "demo@bloodconnect.com"
            };
        }

        private string GetDemoPhone(string userId)
        {
            return userId switch
            {
                "demo-member" => "(555) 000-0002",
                _ => "(555) 000-0000"
            };
        }

        private string GetDemoBloodType(string userId)
        {
            return userId switch
            {
                "demo-member" => "A+",
                _ => "O+"
            };
        }
    }
}
