using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.PatientDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.CheckUpListManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.LabTestManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.PatientManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // Add this

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly ICheckUpService _checkUpService;
        private readonly IPatientTestService _patientTestService;
        private readonly IAdmissionService _admissionService;
        private readonly IDoctorDashboardService _doctorDashboardService;

        public DoctorController(IMapper mapper, IUserService userService, IAppointmentService appointmentService, IPatientService patientService, ICheckUpService checkUpService, IPatientTestService patientTestService, IAdmissionService admissionService, IDoctorDashboardService doctorDashboardService)
        {
            _userService = userService;
            _patientTestService = patientTestService;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _mapper = mapper;
            _admissionService = admissionService;
            _doctorDashboardService = doctorDashboardService;
            _checkUpService = checkUpService;
        }

        // ------------------ Get logged-in user ID from Claims ------------------
        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        // ---------------------------------------------------------------------------------------------
        //// -------------- Load Doctor Dashboard After Login Successfully --------------
        //public IActionResult DoctorDashboard()
        //{
        //    ViewBag.HideLayoutElements = true;
        //    return View();
        //}

        // -------------- Load Doctor Dashboard After Login Successfully --------------
        //public async Task<IActionResult> DoctorDashboard()
        //{
        //    var loggedInDoctor = GetLoggedInUserId();
        //    var doctorId = await _userService.GetDoctorsIdAsync(loggedInDoctor);

        //    // Fetch the doctor's name and data for the dashboard
        //    var doctorName = "Dr. John Doe"; 
        //    var totalAppointments = await _doctorDashboardService.GetTotalAppointmentAsync(doctorId);
        //    var completedAppointments = await _doctorDashboardService.GetCompletedAppointmentAsync(doctorId);
        //    var pendingAppointments = await _doctorDashboardService.GetPendingAppointmentAsync(doctorId);
        //    var totalRevenue = await _doctorDashboardService.GetTotalRevenueAsync(doctorId);
        //    var upcomingAppointments = await _doctorDashboardService.GetUpcomingAppointmentAsync(doctorId);

        //    // Pass the fetched data to the view using ViewBag
        //    ViewBag.DoctorName = doctorName;
        //    ViewBag.TotalAppointments = totalAppointments;
        //    ViewBag.CompletedAppointments = completedAppointments;
        //    ViewBag.PendingAppointments = pendingAppointments;
        //    ViewBag.TotalRevenue = totalRevenue;
        //    ViewBag.UpcomingAppointments = upcomingAppointments;
        //    ViewBag.HideLayoutElements = true;  // Keep this flag for UI

        //    return View();
        //}


        //fetch all the loggedIn doctor related patientList
        public async Task<IActionResult> PatientList()
        {
            ViewBag.HideLayoutElements = true;
            var loggedInDoctor = GetLoggedInUserId();
            var doctorId = await _userService.GetDoctorsIdAsync(loggedInDoctor);
            var patientList = await _patientService.GetPatientsByDoctorIdAsync(doctorId);
            var viewModelList = _mapper.Map<List<PatientViewModel>>(patientList);
            return View("~/Views/Patient/PatientList.cshtml", viewModelList);
        }

        //fetch all the loggedIn doctor related patientList
        public async Task<IActionResult> AppointmentList()
        {
            ViewBag.HideLayoutElements = true;
            var loggedInDoctor = GetLoggedInUserId();
            var doctorId = await _userService.GetDoctorsIdAsync(loggedInDoctor);
            var appointmentList = await _appointmentService.GetAppointmentByDoctorIdAsync(doctorId);
            //var viewModelList = _mapper.Map<List<GetAppointmentDTO>>(patientList);
            return View(appointmentList);
        }

        public async Task<IActionResult> CheckUpList()
        {
            ViewBag.HideLayoutElements = true;
            var loggedInDoctor = GetLoggedInUserId();
            var doctorId = await _userService.GetDoctorsIdAsync(loggedInDoctor);
            var checkupList = await _checkUpService.GetCheckUpListByDoctorAsync(doctorId);
            var viewModelList = _mapper.Map<List<CheckUpViewModel>>(checkupList);
            return View("~/Views/Appointment/CheckUpList.cshtml", viewModelList);
        }

        public async Task<IActionResult> PatientTestList()
        {
            ViewBag.HideLayoutElements = true;
            var loggedInDoctor = GetLoggedInUserId();
            var doctorId = await _userService.GetDoctorsIdAsync(loggedInDoctor);
            var PatientTestLists = await _patientTestService.GetPatientTestByDoctorAsync(doctorId);
            var viewModelList = _mapper.Map<List<PatientTestViewModel>>(PatientTestLists);
            return View("~/Views/Admin/PatientTestList.cshtml", viewModelList);
        }

        public async Task<IActionResult> AdmissionList()
        {
            ViewBag.HideLayoutElements = true;
            var loggedInDoctor = GetLoggedInUserId();
            var doctorId = await _userService.GetDoctorsIdAsync(loggedInDoctor);
            var admissionList = await _admissionService.GetAllAdmissionByDoctorAsync(doctorId);
            var viewModelList = _mapper.Map<List<AdmissionViewModel>>(admissionList);
            return View("~/Views/Bed/AdmissionList.cshtml", viewModelList);
        }
    }
}