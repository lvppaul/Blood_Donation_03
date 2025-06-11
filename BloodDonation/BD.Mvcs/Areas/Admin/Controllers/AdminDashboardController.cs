using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BD.Mvcs.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")] // Add authorization if needed
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Admin Dashboard";

            // Sample data - replace with actual data from your services
            var dashboardData = new
            {
                TotalUsers = 1234,
                ActiveDonors = 856,
                PendingRequests = 42,
                BloodUnits = 2567
            };

            return View(dashboardData);
        }
    }
}