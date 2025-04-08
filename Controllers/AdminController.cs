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

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;
        public readonly ISpecializationService _specializationService;

        public AdminController(IMapper mapper, IUserService userService, ISpecializationService specializationService, IDoctorService doctorService)
        {
            _mapper = mapper;
            _userService = userService;
            _doctorService = doctorService;
            _specializationService = specializationService;
        }

        public IActionResult AdminDashboard()
        {
            ViewBag.HideLayoutElements = true;
            return View();
        }

        public IActionResult CreateAdminUser()
        {
            ViewBag.HideLayoutElements = true;
            return PartialView("_UserForm", new UserRegisterDTO());
        }

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

        // Get logged-in user ID from Claims
        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        // List all userss
        public async Task<IActionResult> UserList()
        {
            ViewBag.HideLayoutElements = true;
            var users = await _userService.GetAllUsersAsync();
            var viewModelList = _mapper.Map<List<UserViewModel>>(users);
            return View(viewModelList);
        }


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

        public IActionResult CreateDoctor()
        {
            ViewBag.HideLayoutElements = true;
            return PartialView("_DoctorForm", new UserDoctorDTO());
        }

        [HttpPost]
        public IActionResult CreateDoctor(UserDoctorDTO userDoctorDTO)
        {
            ViewBag.HideLayoutElements = true;
            if (!ModelState.IsValid)
            {
                return View("_DoctorForm", userDoctorDTO);
            }

            int loggedInUser = GetLoggedInUserId();
            _doctorService.AddDoctorAsync(loggedInUser, userDoctorDTO);
            return RedirectToAction("DoctorList", "Admin");
        }

        public async Task<IActionResult> SpecializationDropDown()
        {
            try
            {
                var specializations = await _specializationService.GetAllSpecializationAsync();
                var viewModelList = _mapper.Map<List<GetSpecializationDTO>>(specializations);
                return Json(viewModelList); // Make sure you have a corresponding View
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching specializations.", Error = ex.Message });
            }
        }

        public async Task<IActionResult> DoctorList()
        {
            ViewBag.HideLayoutElements = true;
            var doctors = await _doctorService.GetAllDoctorAsync();
            var viewModelList = _mapper.Map<List<DoctorViewModel>>(doctors);
            return View(viewModelList);
        }

        public IActionResult CreateReceptionist()
        {
            ViewBag.HideLayoutElements = true;
            return PartialView("_ReceptionistForm", new UserReceptionistDTO());
        }


        public async Task<IActionResult> ReceptionistList()
        {
            ViewBag.HideLayoutElements = true;
            var doctors = await .GetAllReceptionistAsync();
            var viewModelList = _mapper.Map<List<DoctorViewModel>>(doctors);
            return View(viewModelList);
        }


        public async Task<IActionResult> SpecializationList()
        {
            try
            {
                var specializations = await _specializationService.GetAllSpecializationAsync();
                var viewModelList = _mapper.Map<List<GetSpecializationDTO>>(specializations);
                return View(viewModelList); // Make sure you have a corresponding View
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching specializations.", Error = ex.Message });
            }
        }


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