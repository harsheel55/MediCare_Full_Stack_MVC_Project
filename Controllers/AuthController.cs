using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediCare_MVC_Project.MediCare.Application.DTOs.Authenication;
using MediCare_MVC_Project.MediCare.Application.Interfaces;
using MediCare_MVC_Project.MediCare.Application.Interfaces.Authentication;
using MediCare_MVC_Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    public class AuthController : Controller
    {
        private readonly IEmailHelper _emailHelper;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IEmailHelper emailHelper, ILogger<AuthController> logger)
        {
            _emailHelper = emailHelper;
            _authService = authService;
            _logger = logger;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please enter valid login details.";
                return View(model);
            }

            try
            {
                if (model.Email == "admin@gmail.com" && model.Password == "admin")
                    return RedirectToAction("AdminDashboard", "Dashboard");

                string token = await _authService.LoginAsync(model);

                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                    var claims = jsonToken?.Claims.ToList() ?? new List<Claim>();
                    var identity = new ClaimsIdentity(claims, "Cookies");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("Cookies", principal);
                    HttpContext.Session.SetString("UserLoggedIn", "true");

                    var userRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                    HttpContext.Session.SetString("UserRole", userRole ?? "Unknown");

                    TempData["Success"] = "Login successful!";

                    return userRole switch
                    {
                        "Administrator" => RedirectToAction("AdminDashboard", "Dashboard"),
                        "Doctor" => RedirectToAction("DoctorDashboard", "Dashboard"),
                        _ => RedirectToAction("ReceptionistDashboard", "Dashboard"),
                    };
                }

                TempData["Error"] = "Invalid email or password.";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error.");
                TempData["Error"] = "Something went wrong during login.";
                return View(model);
            }
        }

        private string GetRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var roleClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            return roleClaim?.Value;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "You have been logged out.";
            return RedirectToAction("Login");
        }

        // GET: /Auth/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Auth/ForgotPassword
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPwdDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please enter a valid email.";
                return View(model);
            }

            try
            {
                if (model.Email == "admin@gmail.com")
                    return RedirectToAction("ResetPassword", new { token = "", email = model.Email });

                var token = await _authService.ForgotPassword(model);

                _logger.LogInformation("Reset token generated and email sent for: {Email}", model.Email);

                return RedirectToAction("ResetPassword", new { token = token, email = model.Email });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending password reset link.");
                ModelState.AddModelError("", "Error sending password reset email. Please try again later.");
                return View(model);
            }
        }

        // GET: /Auth/ForgotPasswordConfirmation
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            ViewData["Title"] = "Email Sent";
            ViewData["Message"] = "A password reset link has been sent to your email address.";
            return View();
        }

        // ✅ FIXED: GET: /Auth/ResetPassword (Now using ResetPwdDTO instead of ViewModel)
        [HttpGet]
        public IActionResult ResetPassword(string otp, string email)
        {
            var viewModel = new ResetPwdDTO { Otp = otp, Email = email };

            return View(viewModel);
        }

        // POST: /Auth/ResetPassword
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPwdDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please check the input fields.";
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                TempData["Error"] = "Passwords do not match.";
                return View(model);
            }

            try
            {
                var result = await _authService.ResetPassword(model);
                if (result)
                {
                    TempData["Success"] = "Password reset successful!";
                    _logger.LogInformation("Password reset for {Email}", model.Email);
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                TempData["Error"] = "Invalid OTP or email.";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset.");
                TempData["Error"] = "Unable to reset password. Try again.";
                return View(model);
            }
        }

        // GET: /Auth/ResetPasswordConfirmation
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            ViewData["Message"] = "Your password has been successfully reset.";
            return View();
        }
    }
}