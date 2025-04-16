using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Add this

namespace MediCare_MVC_Project.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly IMapper _mapper;

        public DoctorController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // ------------------ Get logged-in user ID from Claims ------------------
        private int GetLoggedInUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        // ---------------------------------------------------------------------------------------------
        // -------------- Load Doctor Dashboard After Login Successfully --------------
        public IActionResult DoctorDashboard()
        {
            ViewBag.HideLayoutElements = true;
            return View();
        }
    }
}