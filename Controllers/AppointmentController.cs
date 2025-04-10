using MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> SendReminder(int id)
        {
            try
            {
                await _appointmentService.SendReminderEmailAsync(id);

                TempData["Success"] = "Reminder Email send Successfully.";

                return RedirectToAction("AppointmentList", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while sending reminder mail.", Error = ex.Message });
            }
        }
    }
}