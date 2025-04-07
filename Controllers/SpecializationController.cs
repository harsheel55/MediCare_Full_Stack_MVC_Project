using System.Security.Claims;
using MediCare_MVC_Project.MediCare.Application.Interfaces.SpecializationManagement;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    public class SpecializationController : Controller
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        private int GetLoggedInUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
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

                return RedirectToAction("SpecializationList", "Admin");
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
                return RedirectToAction("SpecializationList", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error while deleting specialization.", Error = ex.Message });
            }
        }
    }
}