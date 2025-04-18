using System.Security.Claims;
using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement;
using MediCare_MVC_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public RoomController(IRoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        // ------------------ Get logged-in user ID from Claims ------------------
        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        public async Task<IActionResult> RoomList()
        {
            ViewBag.HideLayoutElements = true;

            try
            {
                var roomList = await _roomService.GetAllRoomAsync();
                var viewModelList = _mapper.Map<List<RoomViewModel>>(roomList);
                return View(viewModelList);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Unable to fetch room list.";
                return View(new List<RoomViewModel>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom(RoomDTO room)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill all the required fields correctly.";
                return RedirectToAction("RoomList");
            }

            try
            {
                int loggedUser = GetLoggedInUserId();
                await _roomService.AddNewRoomAsync(loggedUser, room);

                TempData["SuccessMessage"] = "Room added successfully!";
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                TempData["ErrorMessage"] = "Something went wrong while adding the room.";
            }

            return RedirectToAction("RoomList");
        }

        public async Task<IActionResult> UpdateRoom(int roomId, int roomNo, string roomType)
        {
            try
            {
                if (string.IsNullOrEmpty(roomType))
                {
                    return BadRequest("Room type name is required.");
                }

                await _roomService.UpdateRoomAsync(roomId, roomNo, roomType);

                return RedirectToAction("RoomList", "Room");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error while updating room.", Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoom(int roomNo)
        {
            if (roomNo <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Room ID.";
                return RedirectToAction("RoomList");
            }

            try
            {
                await _roomService.DeleteRoomAsync(roomNo);
                TempData["SuccessMessage"] = "Room deleted successfully!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to delete the room.";
            }

            return RedirectToAction("RoomList");
        }
    }
}
