using MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    public class DashboardController : Controller
    {
        // Admin Dashboard
        public IActionResult AdminDashboard()
        {
            ViewBag.HideLayoutElements = true;
            return View("~/Views/Admin/AdminDashboard.cshtml");
        }

        //// Doctor Dashboard
        //public IActionResult DoctorDashboard()
        //{
        //    // Your logic here for Doctor Dashboard
        //    return View("~/Views/Doctor/DoctorDashboard.cshtml");
        //}

        // Receptionist Dashboard
        public IActionResult ReceptionistDashboard()
        {
            // Your logic here for Receptionist Dashboard
            return View("~/Views/Receptionist/ReceptionistDashboard.cshtml");
        }

        private readonly IUserService _userService;
        private readonly IDoctorDashboardService _doctorDashboardService;

        public DashboardController(IUserService userService, IDoctorDashboardService doctorDashboardService)
        {
            _userService = userService;
            _doctorDashboardService = doctorDashboardService;
        }

        // Doctor Dashboard
        public async Task<IActionResult> DoctorDashboard()
        {
            var loggedInDoctor = GetLoggedInUserId();
            var doctorId = await _userService.GetDoctorsIdAsync(loggedInDoctor);

            // Fetch the doctor's name and data for the dashboard
            var doctorName = "Manoj Gajera"; // Replace with actual logic to get doctor's name
            var totalAppointments = await _doctorDashboardService.GetTotalAppointmentAsync(doctorId);
            var completedAppointments = await _doctorDashboardService.GetCompletedAppointmentAsync(doctorId);
            var pendingAppointments = await _doctorDashboardService.GetPendingAppointmentAsync(doctorId);
            var totalRevenue = await _doctorDashboardService.GetTotalRevenueAsync(doctorId);
            var upcomingAppointments = await _doctorDashboardService.GetUpcomingAppointmentAsync(doctorId);

            // Pass the fetched data to the view using ViewBag
            ViewBag.DoctorName = doctorName;
            ViewBag.TotalAppointments = totalAppointments;
            ViewBag.CompletedAppointments = completedAppointments;
            ViewBag.PendingAppointments = pendingAppointments;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.UpcomingAppointments = upcomingAppointments;
            ViewBag.HideLayoutElements = true;  // Keep this flag for UI

            return View("~/Views/Doctor/DoctorDashboard.cshtml");
        }

        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}