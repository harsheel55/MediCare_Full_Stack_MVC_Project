using System.Security.Claims;
using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.CheckUpListManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.LabTestManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.PatientManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement;
using MediCare_MVC_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles = "Receptionist")]
    public class ReceptionistController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly ICheckUpService _checkUpService;
        private readonly IPatientTestService _patientTestService;
        private readonly IAdmissionService _admissionService;

        public ReceptionistController(IMapper mapper, IAppointmentService appointmentService, IPatientService patientService, ICheckUpService checkUpService, IPatientTestService patientTestService, IAdmissionService admissionService)
        {
            _patientTestService = patientTestService;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _mapper = mapper;
            _admissionService = admissionService;
            _checkUpService = checkUpService;
        } 
        
        // ------------------ Get logged-in user ID from Claims ------------------
        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        public async Task<IActionResult> PatientTestList()
        {
            ViewBag.HideLayoutElements = true;
            var PatientTestLists = await _patientTestService.GetAllPatientTestAsync();
            var viewModelList = _mapper.Map<List<PatientTestViewModel>>(PatientTestLists);
            return View("~/Views/Admin/PatientTestList.cshtml", viewModelList);
        }
    }
}
