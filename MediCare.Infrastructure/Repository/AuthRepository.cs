using MediCare_MVC_Project.MediCare.Application.Interfaces;
using MediCare_MVC_Project.MediCare.Application.Interfaces.Authentication;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class AuthRepository : GenericRepository<User>, IAuthRepository
    {
        private readonly IEmailHelper _emailService;

        public AuthRepository(IEmailHelper emailService, ApplicationDBContext context) : base(context)
        {
            _emailService = emailService;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(s => s.Email == email);

                if (user == null || !user.Active)
                    throw new Exception("USER NOT FOUND.");

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateResetToken()
        {
            // Generate a reset token (Base64-encoded GUID)
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 20);
            string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return encodedToken;
        }

        public async Task<bool> UpdatePasswordDB(string newPassword, User user)
        {
            try
            {
                var passwordHasher = new PasswordHasher<User>();
                user.Password = passwordHasher.HashPassword(user, newPassword);

                _context.Users.Update(user);

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}