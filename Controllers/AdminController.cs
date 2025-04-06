using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement;

namespace MediCare_MVC_Project.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult AdminDashboard()
        {
            ViewBag.HideLayoutElements = true;
            return View("~/Views/Admin/AdminDashboard.cshtml");
        }
    }
}