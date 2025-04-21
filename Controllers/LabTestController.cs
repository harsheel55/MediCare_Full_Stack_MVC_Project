using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs.LabTestManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.LabTestManagement;
using MediCare_MVC_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles ="Administrator, Doctor")]
    public class LabTestController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILabTestService _labTestService;
        private readonly IPatientTestService _patientTestService;
        private readonly IAppointmentService _appointmentService;

        public LabTestController(IMapper mapper, ILabTestService labTestService, IPatientTestService patientTestService, IAppointmentService appointmentService)
        {
            _mapper = mapper;
            _appointmentService = appointmentService;
            _labTestService = labTestService;
            _patientTestService = patientTestService;
        }

        [HttpGet]
        public IActionResult CreateLabTest(string AadharNo)
        {
            ViewBag.HideLayoutElements = true;
            return View("_LabTestForm"); // Partial or full view
        }

        [HttpPost]
        public async Task<IActionResult> CreateLabTest(PatientTestDTO patientTestDTO)
        {
            if (!ModelState.IsValid)
                return View("_LabTestForm", patientTestDTO);

            // Save the patient test data
            await _patientTestService.AddPatientTestAsync(patientTestDTO);

            if (User.IsInRole("Doctor"))
            {
                return RedirectToAction("CheckUpList", "Doctor");
            }

            // Redirect to AppointmentList action of Appointment controller
            return RedirectToAction("AppointmentList", "Appointment");
        }

        [HttpGet]
        public async Task<IActionResult> LabTestListDropDown()
        {
            var TestLists = await _labTestService.GetAllTestAsync();
            var viewModelList = _mapper.Map<List<LabTestViewModel>>(TestLists);
            return Json(viewModelList); 
        }

        [HttpPost("UpdateLabTest")]
        public async Task<IActionResult> UpdateLabTest(int TestId, string TestName, string Description, decimal Cost)
        {
            if (string.IsNullOrEmpty(TestName) || string.IsNullOrEmpty(Description) || Cost <= 0)
                return RedirectToAction("LabTestList", "Admin");

            await _labTestService.UpdateLabTestAsync(TestId, TestName, Description, Cost);
            return RedirectToAction("LabTestList", "Admin");
        }

    }
}
