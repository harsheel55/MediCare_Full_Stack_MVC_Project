using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;
using MediCare_MVC_Project.Models;
using MediCare_MVC_Project.MediCare.Application.DTOs;

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // Get logged-in user ID from Claims
        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        // ✅ View user details
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }


        // ✅ CREATE - DOCTOR
        public IActionResult CreateDoctor()
        {
            ViewBag.Action = "CreateDoctor";
            ViewBag.Controller = "User";
            return View("_UserForm", new UserRegisterDTO { RoleId = 2 });
        }

        // ✅ CREATE - RECEPTIONIST
        public IActionResult CreateReceptionist()
        {
            ViewBag.Action = "CreateReceptionist";
            ViewBag.Controller = "User";
            return View("_UserForm", new UserRegisterDTO { RoleId = 3 });
        }

        // ✅ POST - CREATE (for all roles)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRegisterDTO dto)
        {
            if (ModelState.IsValid)
            {
                if (dto.RoleId == 1) // Admin role
                    await _userService.AddUserAsync(GetLoggedInUserId(), dto);
                else if (dto.RoleId == 2) // Doctor role
                    await _userService.AddUserAsync(GetLoggedInUserId(), dto);
                else if (dto.RoleId == 3) // Receptionist role
                    await _userService.AddUserAsync(GetLoggedInUserId(), dto);

                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction("Index");
            }

            return View("_UserForm", dto); // Return form with validation errors
        }

        // ✅ SHARED EDIT for all roles
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            ViewBag.Action = "Edit";
            ViewBag.Controller = "User";

            var dto = _mapper.Map<UserRegisterDTO>(user);
            return View("_UserForm", dto);
        }

        // ✅ POST - Edit User (for all roles)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserRegisterDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _userService.UpdateUserAsync(GetLoggedInUserId(), id, dto);
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction("Index");
            }

            return View("_UserForm", dto); // Return form with validation errors
        }

        // ✅ CONFIRM DELETE view
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return PartialView("_DeleteUser", user);
        }

        // ✅ DELETE - confirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteUserAsync(id);
            TempData["SuccessMessage"] = "User deleted successfully!";
            return Json(new { success = true });
        }
    }
}
