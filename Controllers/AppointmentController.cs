using System.Security.Claims;
using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs;
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
        private readonly IMapper _mapper;
        private readonly IAppointmentService _appointmentService;
        private readonly ICheckUpService _checkUpService;

        public AppointmentController(IAppointmentService appointmentService, ICheckUpService checkUpService, IMapper mapper)
        {
            _mapper = mapper;
            _checkUpService = checkUpService;
            _appointmentService = appointmentService;
        }

        // ------------------ Get logged-in user ID from Claims ------------------
        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }


        // ---------------------------------------------------------------------------------------------
        // -------------- Load Appointment Form  --------------
        [HttpGet("BookAppointment")]
        public IActionResult BookAppointment(int patientId)
        {
            var model = new AppointmentDTO
            {
                PatientId = patientId,
            };

            return PartialView("_AppointmentForm", model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(AppointmentDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AppointmentForm", model);
            }

            int loggedUser = GetLoggedInUserId();
            await _appointmentService.BookAppointmentAsync(loggedUser, model);

            return RedirectToAction("AppointmentList", "Appointment");
        }

        [HttpGet("AppointmentList")]
        public async Task<IActionResult> AppointmentList()
        {
            ViewBag.HideLayoutElements = true;
            var appointmentList = await _appointmentService.GetAllAppointmentAsync();
            var viewModelList = _mapper.Map<List<AppointmentViewModel>>(appointmentList);
            return View(viewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _appointmentService.DeleteAppointmentByIdAsync(id);

                TempData["Success"] = "Appointment Record Deleted Successfully.";

                return RedirectToAction("AppointmentList", "Appointment");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error while deleting Appointment record.", Error = ex.Message });
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> SendReminder(int id)
        {
            try
            {
                await _appointmentService.SendReminderEmailAsync(id);

                TempData["Success"] = "Reminder Email send Successfully.";

                return RedirectToAction("AppointmentList", "Appointment");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while sending reminder mail.", Error = ex.Message });
            }
        }

        // ---------------------------------------------------------------------------------------------
        [HttpGet("CheckUpList")]
        public async Task<IActionResult> CheckUpList()
        {
            ViewBag.HideLayoutElements = true;
            var checkupList = await _checkUpService.GetAllCheckUpAsync();
            var viewModelList = _mapper.Map<List<CheckUpViewModel>>(checkupList);
            return View(viewModelList);
        }

        public async Task<IActionResult> DeleteCheckUp(int id)
        {
            try
            {
                await _checkUpService.DeletePatientNotesAsync(id);
                return RedirectToAction("CheckUpList", "Appointment");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error while deleting Patient's note record.", Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Checkup(int id)
        {
            var viewModel = await _checkUpService.GetCheckupFormDataAsync(id);
            if (viewModel == null)
            {
                TempData["Error"] = "Appointment not found.";
                return RedirectToAction("CheckUpList", "Appointment");
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
            return RedirectToAction("CheckUpList", "Appointment");
        }

        public async Task<IActionResult> DownloadNotes(int id)
        {
            if (id == 0)
                return NotFound();

            var pdfBytes = await _checkUpService.DownloadNotesPdfAsync(id);

            if (pdfBytes == null || pdfBytes.Length == 0)
                return NotFound("PDF generation failed or empty file.");

            return File(pdfBytes, "application/pdf", $"Patient_Notes_{id}.pdf");
        }

        public async Task<IActionResult> SendEmailWithNotes(int id)
        {
            if (id == null)
                return NotFound();

            var emailStatus = await _checkUpService.SendPatientNotePdfAsync(id);

            if (emailStatus)
            {
                TempData["Success"] = "Patient note sent successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to send email.";
            }

            return RedirectToAction("CheckUpList");
        }
    }
}