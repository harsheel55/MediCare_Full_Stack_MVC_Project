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

        // Doctor Dashboard
        public IActionResult DoctorDashboard()
        {
            // Your logic here for Doctor Dashboard
            return View("~/Views/Doctor/DoctorDashboard.cshtml");
        }

        // Receptionist Dashboard
        public IActionResult ReceptionistDashboard()
        {
            // Your logic here for Receptionist Dashboard
            return View("~/Views/Receptionist/ReceptionistDashboard.cshtml");
        }
    }
}