using System.Security.Claims;
using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.PatientDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.PatientManagement;
using MediCare_MVC_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles ="Administrator, Receptionist")]
    public class PatientController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService, IMapper mapper)
        {
            _mapper = mapper;
            _patientService = patientService;
        }

        // ------------------ Get logged-in user ID from Claims ------------------
        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
        // ---------------------------------------------------------------------------------------------
        // -------------- Load _PatientForm form creating Patient --------------

        public IActionResult CreatePatient()
        {
            ViewBag.HideLayoutElements = true;
            return PartialView("_PatientForm", new GetPatientDTO());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient(GetPatientDTO userDto)
        {
            ViewBag.HideLayoutElements = true;

            if (!ModelState.IsValid)
            {
                return View("_PatientForm", userDto);
            }

            int loggedInUser = GetLoggedInUserId();
            await _patientService.AddPatientAsync(loggedInUser, userDto);

            return RedirectToAction("PatientList", "Patient");
        }

        // -------------- Show all the Patient User list in Patient Module --------------
        public async Task<IActionResult> PatientList()
        {
            ViewBag.HideLayoutElements = true;
            var patientList = await _patientService.GetAllPatientAsync();
            var viewModelList = _mapper.Map<List<PatientViewModel>>(patientList);
            return View(viewModelList);
        }

        public async Task<IActionResult> DeletePatient(int id)
        {
            try
            {
                await _patientService.DeletePatientAsync(id);
                return RedirectToAction("PatientList", "Patient");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error while deleting Patient record.", Error = ex.Message });
            }
        }

        // -------------- Fill form for update data --------------
        [HttpGet]
        public async Task<IActionResult> EditPatient(int id)
        {
            var userDto = await _patientService.GetPatientByIdAsync(id);
            if (userDto == null)
            {
                return NotFound();
            }
            //var userRegisterDto = _mapper.Map<GetPatientDTO>(userDto);
            return View("_PatientForm", userDto);
        }

        // -------------- Update data into Database --------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPatient(int id, GetPatientDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("_PatientForm", dto);
            }

            var loggedInUser = GetLoggedInUserId();
            await _patientService.UpdatePatientAsync(id, dto, loggedInUser);
            return RedirectToAction("PatientList", "Patient");
        }
    }
}