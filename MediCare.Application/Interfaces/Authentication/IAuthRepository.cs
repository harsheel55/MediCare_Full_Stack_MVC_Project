using MediCare_MVC_Project.MediCare.Domain.Entity;

namespace MediCare_MVC_Project.MediCare.Application.Interfaces.Authentication
{
    public interface IAuthRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        string GenerateResetToken();
        Task<bool> UpdatePasswordDB(string newPassword, User user);
    }
}