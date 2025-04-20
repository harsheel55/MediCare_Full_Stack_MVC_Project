using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdmissionController : Controller
    {
        private readonly IAdmissionService _admissionService;
        private readonly IMapper _mapper;

        public AdmissionController(IAdmissionService admissionService, IMapper mapper
            )
        {
            _mapper = mapper;
            _admissionService = admissionService;
        }

        // -------------- Fill form for update data --------------
        [HttpGet]
        public async Task<IActionResult> EditAdmission(int id)
        {
            var admissionDto = await _admissionService.GetAdmissionRecordsByIdAsync(id);
            if (admissionDto == null)
            {
                return NotFound();
            }

            var updateDto = _mapper.Map<AdmissionUpdateDTO>(admissionDto);
            return View("_AdmissionUpdateForm", updateDto);
        }

        // -------------- Update data into Database --------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmission(int id, AdmissionUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("_AdmissionUpdateForm", dto);
            }

            await _admissionService.UpdateAdmissionRecordAsync(id, dto);
            return RedirectToAction("AdmissionList", "Bed");
        }

        public async Task<IActionResult> DeleteAdmission(int id)
        {
            if (id <= 0)
                throw new Exception("AdmissionId is invalid.");

            await _admissionService.DeleteAdmissionRecordAsync(id);
            return RedirectToAction("AdmissionList", "Bed");
        }
    }
}
