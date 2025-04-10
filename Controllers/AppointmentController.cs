using System.Security.Claims;
using MediCare_MVC_Project.MediCare.Application.DTOs.CheckUpDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.CheckUpListManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.Controllers
{
    //[Authorize(Roles ="Administrator, Doctor")]
    [Route("Appointment")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ICheckUpService _checkUpService;

        public AppointmentController(IAppointmentService appointmentService, ICheckUpService checkUpService)
        {
            _checkUpService = checkUpService;
            _appointmentService = appointmentService;
        }

        // ------------------ Get logged-in user ID from Claims ------------------
        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
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

        [HttpGet]
        public async Task<IActionResult> Checkup(int id)
        {
            var viewModel = await _checkUpService.GetCheckupFormDataAsync(id);
            if (viewModel == null)
            {
                TempData["Error"] = "Appointment not found.";
                return RedirectToAction("CheckUpList", "Admin");
            }

            return View("_CheckUpForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkup(CheckUpDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View("_CheckUpForm", model);
            }
            var createdBy = GetLoggedInUserId(); 
            var success = await _checkUpService.AddPatientNoteAsync(model, createdBy);
            if (success)
            {
                TempData["Success"] = "Note saved successfully!";
            }
            else
                TempData["Error"] = "Please provide valid input.";
            return RedirectToAction("CheckUpList", "Admin");
        }

    }
}