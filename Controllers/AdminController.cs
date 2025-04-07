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

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public readonly ISpecializationService _specializationService;

        public AdminController(IMapper mapper, IUserService userService, ISpecializationService specializationService)
        {
            _mapper = mapper;
            _userService = userService;
            _specializationService = specializationService;
        }

        public IActionResult AdminDashboard()
        {
            ViewBag.HideLayoutElements = true;
            return View();
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

        [HttpGet]
        public IActionResult CreateAdminUser()
        {
            ViewBag.HideLayoutElements = true;
            var userDto = new UserRegisterDTO();
            return View(); // This will look for Views/Admin/CreateAdminUser.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdminUser(UserRegisterDTO userDto)
        {
            ViewBag.HideLayoutElements = true;

            if (!ModelState.IsValid)
            {
                return View("_UserForm", userDto); // Match your actual view name
            }

            int loggedInUser = GetLoggedInUserId();
            await _userService.AddUserAsync(loggedInUser, userDto);

            return RedirectToAction("UserList");
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