using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;
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

        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        //public async Task<IActionResult> EditAdmin(int id)
        //{
        //    await _userService.UpdateUserAsync(GetLoggedInUserId(), id, );
        //    TempData["SuccessMessage"] = "User updated successfully!";
        //    return RedirectToAction("UserList", "Admin");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmin(int id, UserRegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {

                return View("_UserForm", dto);
            }
            await _userService.UpdateUserAsync(GetLoggedInUserId(), id, dto);
            TempData["SuccessMessage"] = "User updated successfully!";
            return RedirectToAction("UserList", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                await _userService.DeleteUserAsync(email);
            }

            return RedirectToAction("UserList", "Admin");
        }
    }
}
