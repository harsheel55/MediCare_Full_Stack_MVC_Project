using System.Diagnostics;
using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using MediCare_MVC_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailHelper _emailHelper;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext context, IEmailHelper emailHelper)
        {
            _logger = logger;
            _context = context;
            _emailHelper = emailHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactUsDTO contactUsDTO)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields correctly.";
                return RedirectToAction("Contact");
            }

            var contact = new ContactUs
            {
                Name = contactUsDTO.Name,
                Email = contactUsDTO.Email,
                Phone = contactUsDTO.Phone,
                QueryType = contactUsDTO.QueryType,
                Message = contactUsDTO.Message,
                SubmittedAt = DateTime.Now
            };

            try
            {
                // Save to DB
                _context.ContactUs.Add(contact);
                _context.SaveChanges();

                // Send email
               
                _emailHelper.SendUserQueryAcknowledgementEmailAsync(contactUsDTO);

                TempData["Success"] = "Your message has been submitted successfully!";
                _logger.LogInformation("Contact form submitted by {Email}", contact.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while submitting contact form.");
                TempData["Error"] = "An error occurred while submitting your message. Please try again later.";
            }

            return RedirectToAction("Contact");
        }

        public IActionResult Login()
        {
            return RedirectToAction("Login", "Auth");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}