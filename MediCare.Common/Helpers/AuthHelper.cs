using MediCare_MVC_Project.MediCare.Application.DTOs.Authenication;
using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.Authentication;
using MediCare_MVC_Project.MediCare.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;
using MediCare_MVC_Project.Models;
using System.Text;

namespace MediCare_MVC_Project.MediCare.Common.Helpers
{
    public class AuthHelper : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JWTTokenHelper _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IEmailHelper _emailService;

        // In-memory OTP store (use DB in production)
        private static readonly ConcurrentDictionary<string, (string Otp, DateTime Expiry)> _otpStore = new();

        public AuthHelper(IAuthRepository authRepository, IEmailHelper emailService, JWTTokenHelper tokenService, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _emailService = emailService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginViewModel userLogin)
        {
            try
            {
                var user = await _authRepository.GetUserByEmailAsync(userLogin.Email);
                if (user == null)
                    throw new Exception("USER NOT FOUND");

                var passwordHasher = new PasswordHasher<LoginViewModel>();
                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(userLogin, user.Password, userLogin.Password);

                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                    throw new Exception("Invalid email or password");

                string token = _tokenService.GenerateToken(userLogin, user.RoleId, user.UserId, user.FirstName, user.LastName);
                Console.WriteLine($"Generated Token: {token}");

                return token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> ForgotPassword(ForgotPwdDTO forgotPwd)
        {
            var user = await _authRepository.GetUserByEmailAsync(forgotPwd.Email);
            if (user == null)
                throw new Exception("User not found.");

            // Generate OTP
            var otp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(10);

            // Store OTP in memory (key: email)
            _otpStore[forgotPwd.Email] = (otp, expiry);
            Console.WriteLine("Forgot Password OTP: " + otp);
            var emailBody = $@"
                <h2>Password Reset OTP</h2>
                <p>Use the following OTP to reset your password:</p>
                <h3>{otp}</h3>
                <p>This OTP will expire in 10 minutes.</p>";

            await _emailService.SendEmailAsync(user.Email, "Reset Password OTP", emailBody);
            return "OTP has been sent to your registered email.";
        }

        public async Task<bool> ResetPassword(ResetPwdDTO resetPwd)
        {
            try
            {
                if (string.IsNullOrEmpty(resetPwd.Email) || string.IsNullOrEmpty(resetPwd.Otp))
                    throw new Exception("Email and OTP are required.");

                // Validate OTP
                if (!_otpStore.TryGetValue(resetPwd.Email, out var otpData))
                    throw new Exception("OTP not found or expired.");

                if (otpData.Expiry < DateTime.UtcNow || otpData.Otp != resetPwd.Otp)
                    throw new Exception("Invalid or expired OTP.");

                var user = await _authRepository.GetUserByEmailAsync(resetPwd.Email);
                if (user == null)
                    throw new Exception("User not found.");

                var result = await _authRepository.UpdatePasswordDB(resetPwd.ConfirmPassword, user);
                if (!result)
                    throw new Exception("Failed to update password.");

                // Remove OTP after successful use
                _otpStore.TryRemove(resetPwd.Email, out _);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error resetting password.", ex);
            }
        }
    }
}