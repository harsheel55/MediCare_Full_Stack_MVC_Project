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
            var roomList = await _roomService.GetAllRoomAsync();
            var viewModelList = _mapper.Map<List<RoomViewModel>>(roomList);
            return View(viewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom(RoomDTO room)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("RoomList");
            }

            int loggedUser = GetLoggedInUserId();
            await _roomService.AddNewRoomAsync(loggedUser, room);
            return RedirectToAction("RoomList");
        }

        //public async Task<IActionResult> Delete(int roomId)
        //{
        //    if (roomId == 0)
        //        throw new Exception("Id is not valid");

        //}
    }
}
