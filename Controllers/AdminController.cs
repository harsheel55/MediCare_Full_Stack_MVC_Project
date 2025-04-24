using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement;
using MediCare_MVC_Project.MediCare.Application.Services;
using MediCare_MVC_Project.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using MediCare_MVC_Project.MediCare.Application.Interfaces.SpecializationManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Application.DTOs.SpecializationDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.DoctorManagement;
using MediCare_MVC_Project.MediCare.Application.DTOs.DoctorDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.ReceptionistManagement;
using MediCare_MVC_Project.MediCare.Application.DTOs.PatientDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.PatientManagement;
using MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.CheckUpListManagement;
using MediCare_MVC_Project.MediCare.Application.DTOs.CheckUpDTOs;
using Microsoft.EntityFrameworkCore;
using MediCare_MVC_Project.MediCare.Application.Interfaces.LabTestManagement;
using MediCare_MVC_Project.MediCare.Application.DTOs.LabTestManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.PaymentManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement;
using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;
using Microsoft.IdentityModel.Tokens;

namespace MediCare_MVC_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly ICheckUpService _checkUpService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly ILabTestService _labTestService;
        private readonly IPaymentService _paymentService;
        private readonly IPatientTestService _patientTestService;
        private readonly IAppointmentService _appointmentService;
        private readonly IReceptionistService _receptionistService;
        private readonly ISpecializationService _specializationService;

        public AdminController(IMapper mapper, IUserService userService, ISpecializationService specializationService, IDoctorService doctorService, IReceptionistService receptionistService, IPatientService patientService, IAppointmentService appointmentService, ICheckUpService checkUpService, ILabTestService labTestService, IPatientTestService patientTestService, IPaymentService paymentService, IRoomService roomService)
        {
            _mapper = mapper;
            _userService = userService;
            _roomService = roomService;
            _doctorService = doctorService;
            _labTestService = labTestService;
            _checkUpService = checkUpService;
            _patientService = patientService;
            _paymentService = paymentService;
            _patientTestService = patientTestService;
            _appointmentService = appointmentService;
            _receptionistService = receptionistService;
            _specializationService = specializationService;
        }

        // ------------------ Get logged-in user ID from Claims ------------------
        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        // ---------------------------------------------------------------------------------------------
        // -------------- Load Admin Dashboard After Login Successfully --------------
        //[Authorize(Roles = "Administrator")]
        //public IActionResult AdminDashboard()
        //{
        //    ViewBag.HideLayoutElements = true;
        //    return View();
        //}

        // -------------- Show all the Lab Test list in Lab Test Module --------------
        [Authorize(Roles = "Administrator,Receptionist")]
        public async Task<IActionResult> LabTestList()
        {
            ViewBag.HideLayoutElements = true;
            var TestLists = await _labTestService.GetAllTestAsync();
            var viewModelList = _mapper.Map<List<LabTestViewModel>>(TestLists);
            return View(viewModelList);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddLabTest(LabTestDTO labTest)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("LabTestList");
            }

            await _labTestService.AddNewTestAsync(labTest);
            return RedirectToAction("LabTestList");
        }

        // -------------- Show all the Patient's Lab Test list in Lab Test Module --------------
        [Authorize(Roles = "Administrator,Receptionist")]
        public async Task<IActionResult> PatientTestList()
        {
            ViewBag.HideLayoutElements = true;
            var PatientTestLists = await _patientTestService.GetAllPatientTestAsync();
            var viewModelList = _mapper.Map<List<PatientTestViewModel>>(PatientTestLists);
            return View(viewModelList);
        }

        [Authorize(Roles = "Administrator,Doctor,Receptionist")]
        public async Task<IActionResult> UpdatePatientTest(int patientTestId, DateOnly testDate, string result)
        {
            if (patientTestId <= 0 || testDate == DateOnly.MinValue || string.IsNullOrEmpty(result))
                throw new Exception("Invalid Data.");

            await _patientTestService.UpdatePatientTestAsync(patientTestId, testDate, result);


            if (User.IsInRole("Doctor"))
            {
                return RedirectToAction("PatientTestList", "Doctor");
            }
            else if (User.IsInRole("Receptionist"))
            {
                return RedirectToAction("PatientTestList", "Receptionist");
            }

            return RedirectToAction("PatientTestList", "Admin");
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> DeletePatientTest(int id)
        {
            if (id == 0)
                throw new Exception("TestId is invalid.");

            await _patientTestService.DeletePatientTestAsync(id);
            return RedirectToAction("PatientTestList", "Admin");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PaymentInvoiceList()
        {
            ViewBag.HideLayoutElements = true;
            var invoicesList = await _paymentService.GetAllPaymentAsync();
            var viewModelList = _mapper.Map<List<PaymentInvoiceViewModel>>(invoicesList);
            return View(viewModelList);
        }
    }
}