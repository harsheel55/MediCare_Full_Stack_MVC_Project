using System.Security.Claims;
using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs.SpecializationDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.SpecializationManagement;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    public class SpecializationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISpecializationService _specializationService;

        public SpecializationController(ISpecializationService specializationService, IMapper mapper)
        {
            _mapper = mapper;
            _specializationService = specializationService;
        }

        private int GetLoggedInUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
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


        [HttpPost]
        public async Task<IActionResult> Update(int id, string specializationName)
        {
            try
            {
                if (string.IsNullOrEmpty(specializationName))
                {
                    return BadRequest("Specialization name is required.");
                }

                var updatedById = GetLoggedInUserId();
                await _specializationService.UpdateSpecializationByIdAsync(updatedById, id, specializationName);

                return RedirectToAction("SpecializationList", "Specialization");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error while updating specialization.", Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _specializationService.DeleteSpecializationByIdAsync(id);
                return RedirectToAction("SpecializationList", "Specialization");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error while deleting specialization.", Error = ex.Message });
            }
        }
    }
}