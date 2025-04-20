using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.LabTestManagement;
using MediCare_MVC_Project.MediCare.Application.Services;
using MediCare_MVC_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    public class BedController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBedService _bedService;
        private readonly IRoomService _roomService;
        private readonly IAdmissionService _admissionService;

        public BedController(
            IMapper mapper,
            IBedService bedService,
            IRoomService roomService,
            IAdmissionService admissionService)
        {
            _mapper = mapper;
            _bedService = bedService;
            _roomService = roomService;
            _admissionService = admissionService;
        }

        [HttpGet]
        public async Task<IActionResult> BedAllotmentForm(string aadharNo)
        {
            AdmissionDTO model = new AdmissionDTO();
            ViewBag.HideLayoutElements = true;
            return View("_BedAllotmentForm", model);
        }

        [HttpPost]
        public async Task<IActionResult> BedAllotmentForm(AdmissionDTO model)
        {
            if (!ModelState.IsValid)
                return View("_BedAllotmentForm", model);

            await _admissionService.AddAdmissionRecordsAsync(model);

            return RedirectToAction("AdmissionList", "Bed");
        }

        public async Task<IActionResult> AdmissionList()
        {
            ViewBag.HideLayoutElements = true;
            var admissionList = await _admissionService.GetAllAdmissionRecordsAsync();
            var viewModelList = _mapper.Map<List<AdmissionViewModel>>(admissionList);
            return View(viewModelList);
        }

        public async Task<IActionResult> BedList()
        {
            ViewBag.HideLayoutElements = true;
            var bedList = await _bedService.GetAllBedsAsync();
            var viewModelList = _mapper.Map<List<BedViewModel>>(bedList);
            return View(viewModelList);
        }

        [HttpGet]
        public IActionResult AddBed()
        {
            BedDTO model = new BedDTO();
            ViewBag.HideLayoutElements = true;
            return View("_AddNewBed", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddBed(BedDTO bedDto)
        {
            if (!ModelState.IsValid)
            {
                return View("_BedAllotmentForm", bedDto);
            }

            await _bedService.AddNewBedAsync(bedDto);

            return RedirectToAction("BedList", "Bed");
        }

        [HttpPost]
        public async Task<IActionResult> EditBed(int BedId, int BedNo, int RoomNo, string RoomType, bool IsOccupied)
        {
            await _bedService.UpdateBedAsync(BedId, BedNo, RoomNo, RoomType, IsOccupied);

            return RedirectToAction("BedList", "Bed");
        }

        [HttpGet]
        public async Task<IActionResult> EmptyBed(int BedId)
        {
            if (BedId == 0)
                throw new Exception("Id is not valid.");

            await _bedService.DeleteBedAsync(BedId);
            return RedirectToAction("BedList", "Bed");
        }


        [HttpGet]
        public async Task<IActionResult> GetAvailableBedsDropdown()
        {
            var bedList = await _bedService.GetAllBedsAsync();
            var viewModelList = _mapper.Map<List<GetBedDTO>>(bedList);
            return Json(viewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoomListDropDown()
        {
            ViewBag.HideLayoutElements = true;
            var roomList = await _roomService.GetAllRoomAsync();
            var viewModelList = _mapper.Map<List<RoomViewModel>>(roomList);
            return Json(viewModelList);
        }
    }
}
