using MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    public class DashboardController : Controller
    {
        //// Admin Dashboard
        //public IActionResult AdminDashboard()
        //{
        //    ViewBag.HideLayoutElements = true;
        //    return View("~/Views/Admin/AdminDashboard.cshtml");
        //}

        //// Doctor Dashboard
        //public IActionResult DoctorDashboard()
        //{
        //    // Your logic here for Doctor Dashboard
        //    return View("~/Views/Doctor/DoctorDashboard.cshtml");
        //}

        // Receptionist Dashboard
        //public IActionResult ReceptionistDashboard()
        //{
        //    // Your logic here for Receptionist Dashboard
        //    return View("~/Views/Receptionist/ReceptionistDashboard.cshtml");
        //}

        private readonly IUserService _userService;
        private readonly IDoctorDashboardService _doctorDashboardService;
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly IReceptionistDashboardService _receptionistDashboardService;

        public DashboardController(IUserService userService, IDoctorDashboardService doctorDashboardService, IAdminDashboardService adminDashboardService, IReceptionistDashboardService receptionistDashboardService)
        {
            _userService = userService;
            _receptionistDashboardService = receptionistDashboardService;
            _adminDashboardService = adminDashboardService;
            _doctorDashboardService = doctorDashboardService;
        }
        public async Task<IActionResult> ReceptionistDashboard()
        {
            ViewBag.HideLayoutElements = true;
            var loggedInDoctor = GetLoggedInUserId();

            var userName = await _userService.GetUserByIdAsync(loggedInDoctor);

            var receptionistName = userName.FirstName + " " + userName.LastName;
            var totalAppointment = await _receptionistDashboardService.GetTotalAppointmentCountAsync();
            var totalPatient = await _receptionistDashboardService.GetTotalPatientCountAsync();
            var totalRevenue = await _receptionistDashboardService.GetTotalRevenueAsync();
            var todayAppointment = await _receptionistDashboardService.GetTodaysAppointmentCountAsync();
            var recentAppointment = await _receptionistDashboardService.GetRecentAppointmentAsync();
            var upcomingAppointment = await _receptionistDashboardService.GetUpcomingAppointmentAsync();

            ViewBag.ReceptionistName = receptionistName;
            ViewBag.TotalAppointments = totalAppointment;
            ViewBag.TotalPatients = totalPatient;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.AppointmentsToday = todayAppointment;
            ViewBag.RecentAppointments = recentAppointment;
            ViewBag.UpcomingAppointments = upcomingAppointment;

            return View("~/Views/Receptionist/ReceptionistDashboard.cshtml");
        }

        public async Task<IActionResult> AdminDashboard()
        {
            ViewBag.HideLayoutElements = true;
            var loggedInDoctor = GetLoggedInUserId();
            var userName = await _userService.GetUserByIdAsync(loggedInDoctor);

            var adminName = userName.FirstName + " " + userName.LastName;
            var totalDoctor = await _adminDashboardService.GetTotalDoctorsAsync();
            var totalPatient = await _adminDashboardService.GetTotalPatientsAsync();
            var totalReceptionist = await _adminDashboardService.GetTotalReceptionistsAsync();
            var totalLabTests = await _adminDashboardService.GetTotalLabTestsAsync();
            var todayAppointment = await _adminDashboardService.GetTodayAppointmentsCountAsync();
            var totaRevenue = await _adminDashboardService.GetTotalRevenueAsync();
            var totalAppointment = await _adminDashboardService.GetTotalAppointmentsAsync();
            var availableBad = await _adminDashboardService.GetAvailableBedsCountAsync();
            var recentAppointment = await _adminDashboardService.GetRecentAppointmentsAsync();

            ViewBag.AdminName = adminName;
            ViewBag.TotalDoctors = totalDoctor;
            ViewBag.TotalPatients = totalPatient;
            ViewBag.TotalReceptionists = totalReceptionist;
            ViewBag.TotalLabTests = totalLabTests;
            ViewBag.AppointmentsToday = todayAppointment;
            ViewBag.TotalRevenue = totaRevenue;
            ViewBag.TotalAppointments = totalAppointment;
            ViewBag.AvailableBeds = availableBad;
            ViewBag.RecentAppointments = recentAppointment;

            return View("~/Views/Admin/AdminDashboard.cshtml");
        }

        // Doctor Dashboard
        public async Task<IActionResult> DoctorDashboard()
        {
            var loggedInDoctor = GetLoggedInUserId();
            var doctorId = await _userService.GetDoctorsIdAsync(loggedInDoctor);

            var userName = await _userService.GetUserByIdAsync(loggedInDoctor);

            var doctorName = userName.FirstName + " " + userName.LastName;
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