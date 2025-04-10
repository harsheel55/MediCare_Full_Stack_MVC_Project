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

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        //private readonly ICheckUpService _checkUpService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IReceptionistService _receptionistService;
        private readonly ISpecializationService _specializationService;

        public AdminController(IMapper mapper, IUserService userService, ISpecializationService specializationService, IDoctorService doctorService, IReceptionistService receptionistService, IPatientService patientService, IAppointmentService appointmentService)
        {
            _mapper = mapper;
            _userService = userService;
            _doctorService = doctorService;
            _patientService = patientService;
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
        public IActionResult AdminDashboard()
        {
            ViewBag.HideLayoutElements = true;
            return View();
        }

        // -------------- Load _UserForm form creating Admin User --------------
        public IActionResult CreateAdminUser()
        {
            ViewBag.HideLayoutElements = true;
            return PartialView("_UserForm", new UserRegisterDTO());
        }

        // -------------- Take Data from form to add new Admin User --------------
        [HttpPost]
        public async Task<IActionResult> CreateAdminUser(UserRegisterDTO userDto)
        {
            ViewBag.HideLayoutElements = true;

            if (!ModelState.IsValid)
            {
                return View("_UserForm", userDto);
            }

            int loggedInUser = GetLoggedInUserId();
            await _userService.AddUserAsync(loggedInUser, userDto);

            return RedirectToAction("UserList", "Admin");
        }

        // -------------- Show all the Admin User list in User Module --------------
        public async Task<IActionResult> UserList()
        {
            ViewBag.HideLayoutElements = true;
            var users = await _userService.GetAllUsersAsync();
            var viewModelList = _mapper.Map<List<UserViewModel>>(users);
            return View(viewModelList);
        }

        // -------------- Update Admin User using same _UserForm --------------
        [HttpPut]
        public async Task<IActionResult> EditAdminUser([FromQuery] int id, [FromBody] UserRegisterDTO user)
        {
            try
            {
                if (user == null)
                    return BadRequest(new { Message = "User data is required." });

                var updatedById = GetLoggedInUserId();

                await _userService.UpdateUserAsync(updatedById, id, user);
                return Ok(new { Message = "User updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                throw;
            }
        }


        // ---------------------------------------------------------------------------------------------
        // -------------- Load _DoctorForm form creating Doctor User --------------
        public IActionResult CreateDoctor()
        {
            ViewBag.HideLayoutElements = true;
            return PartialView("_DoctorForm", new UserDoctorDTO());
        }

        // -------------- Take Data from form to add new doctor User --------------
        [HttpPost]
        public async Task<IActionResult> CreateDoctor(UserDoctorDTO userDoctorDTO)
        {
            ViewBag.HideLayoutElements = true;
            if (!ModelState.IsValid)
            {
                return View("_DoctorForm", userDoctorDTO);
            }

            int loggedInUser = GetLoggedInUserId();
            await _doctorService.AddDoctorAsync(loggedInUser, userDoctorDTO);
            return RedirectToAction("DoctorList", "Admin");
        }

        // -------------- Load All the Specialization in _DoctorForm --------------
        public async Task<IActionResult> SpecializationDropDown()
        {
            try
            {
                var specializations = await _specializationService.GetAllSpecializationAsync();
                var viewModelList = _mapper.Map<List<GetSpecializationDTO>>(specializations);
                return Json(viewModelList); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching specializations.", Error = ex.Message });
            }
        }

        // -------------- Show all the Doctor User list in Doctor Module --------------
        public async Task<IActionResult> DoctorList()
        {
            ViewBag.HideLayoutElements = true;
            var doctors = await _doctorService.GetAllDoctorAsync();
            var viewModelList = _mapper.Map<List<DoctorViewModel>>(doctors);
            return View(viewModelList);
        }


        // ---------------------------------------------------------------------------------------------
        // -------------- Load _ReceptionistForm form creating Receptionist User --------------
        public IActionResult CreateReceptionist()
        {
            ViewBag.HideLayoutElements = true;
            return PartialView("_ReceptionistForm", new UserReceptionistDTO());
        }

        // -------------- Take Data from form to add new Receptionist User --------------
        [HttpPost]
        public async Task<IActionResult> CreateReceptionist(UserReceptionistDTO userDto)
        {
            ViewBag.HideLayoutElements = true;

            if (!ModelState.IsValid)
            {
                return View("_ReceptionistForm", userDto);
            }

            int loggedInUser = GetLoggedInUserId();
            await _receptionistService.AddReceptionistAsync(loggedInUser, userDto);

            return RedirectToAction("ReceptionistList", "Admin");
        }

        // -------------- Show all the Receptionist list in Receptionist Module --------------
        public async Task<IActionResult> ReceptionistList()
        {
            ViewBag.HideLayoutElements = true;
            var receptionist = await _receptionistService.GetAllReceptionistAsync();
            var viewModelList = _mapper.Map<List<ReceptionistViewModel>>(receptionist);
            return View(viewModelList);
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

            return RedirectToAction("PatientList", "Admin");
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
                return RedirectToAction("PatientList", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error while deleting Patient record.", Error = ex.Message });
            }
        }



        // ---------------------------------------------------------------------------------------------
        // -------------- Show all the Specialization list in Specialization Module --------------
        public async Task<IActionResult> SpecializationList()
        {
            try
            {
                var specializations = await _specializationService.GetAllSpecializationAsync();
                var viewModelList = _mapper.Map<List<GetSpecializationDTO>>(specializations);
                return View(viewModelList); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching specializations.", Error = ex.Message });
            }
        }

        // -------------- Take Data Input Field from Specialization module to add new Specialization --------------
        [HttpPost]
        public async Task<IActionResult> AddSpecialization(string specializationName)
        {
            if (!string.IsNullOrWhiteSpace(specializationName))
            {
                int loggedInUser = GetLoggedInUserId();
                await _specializationService.AddSpecializationAsync(loggedInUser, specializationName);
            }
            return RedirectToAction("SpecializationList");
        }


        // ---------------------------------------------------------------------------------------------
        // -------------- Load Appointment Form  --------------
        public IActionResult BookAppointment(int patientId)
        {
            var model = new AppointmentDTO
            {
                PatientId = patientId,
            };

            return PartialView("_AppointmentForm", model); // but only works if used inside an already loaded view
        }

        public async Task<IActionResult> GetDoctorsDropdown()
        {
            var doctors = await _doctorService.GetAllDoctorAsync(); // or however you're fetching
            var result = doctors.Select(d => new
            {
                doctorId = d.DoctorId,
                doctorName = d.FirstName + " " + d.LastName,
                specialization = d.Specialization
            });

            return Json(result);
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

            return RedirectToAction("AppointmentList", "Admin");
        }

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

                return RedirectToAction("AppointmentList", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error while deleting Appointment record.", Error = ex.Message });
            }
        }
    }
}