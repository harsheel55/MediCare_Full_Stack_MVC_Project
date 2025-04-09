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

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;
        private readonly IReceptionistService _receptionistService;
        private readonly ISpecializationService _specializationService;

        public AdminController(IMapper mapper, IUserService userService, ISpecializationService specializationService, IDoctorService doctorService, IReceptionistService receptionistService)
        {
            _mapper = mapper;
            _userService = userService;
            _doctorService = doctorService;
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
    }
}