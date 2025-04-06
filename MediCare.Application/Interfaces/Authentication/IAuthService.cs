using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.Authenication;
using MediCare_MVC_Project.Models;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.Authentication
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginViewModel userLogin);
        Task<string> ForgotPassword(ForgotPwdDTO forgotPwd);
        Task<bool> ResetPassword(ResetPwdDTO resetPwd);
    }
}