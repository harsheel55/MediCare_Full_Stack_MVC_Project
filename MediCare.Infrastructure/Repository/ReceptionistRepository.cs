using System.Numerics;
using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.DoctorDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.ReceptionistDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces;
using MediCare_MVC_Project.MediCare.Application.Interfaces.ReceptionistManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class ReceptionistRepository : GenericRepository<Receptionist>, IReceptionistRepository
    {
        private readonly IMapper _mapper;
        private readonly IEmailHelper _emailHelper;

        public ReceptionistRepository(ApplicationDBContext context, IMapper mapper, IEmailHelper emailHelper) : base(context)
        {
            _mapper = mapper;
            _emailHelper = emailHelper;
        }

        public async Task AddReceptionistQuery(int id, UserReceptionistDTO receptionist)
        {
            try
            {
                if (receptionist == null)
                    throw new ArgumentNullException(nameof(receptionist), "Receptionist object is null.");

                if (string.IsNullOrWhiteSpace(receptionist.Email))
                    throw new ArgumentException("Email is required.");

                if (receptionist.RoleId <= 0)
                    throw new ArgumentException("Invalid RoleId.");

                // Check if a user with the same email and role already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == receptionist.Email && u.RoleId == receptionist.RoleId);

                if (existingUser != null)
                {
                    throw new ArgumentException("Receptionist already exists.");
                }

                // STEP 1: Create User entity from DTO
                var newUser = _mapper.Map<User>(receptionist);

                // Hash the password
                var passwordHasher = new PasswordHasher<UserReceptionistDTO>();
                var hashedPassword = passwordHasher.HashPassword(receptionist, receptionist.Password);
                
                newUser.Password = hashedPassword;
                newUser.RoleId = receptionist.RoleId;
                newUser.CreatedBy = id;

                // Save new user to get the generated UserId
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // STEP 2: Use newUser.UserId in Receptionist
                receptionist.Id = newUser.UserId;

                var newReceptionist = _mapper.Map<Receptionist>(receptionist);
                newReceptionist.CreatedAt = DateTime.UtcNow;
                newReceptionist.UserId = newUser.UserId;

                _context.Receptionists.Add(newReceptionist);
                await _context.SaveChangesAsync();

                await _emailHelper.SendUserRegistrationEmailAsync(receptionist.Email, receptionist.Password);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add receptionist: " + ex.Message);
            }
        }
        public async  Task<ICollection<GetReceptionistDTO>> GetAllReceptionistQuery()
        {
            var receptionistList = await _context.Users.Include(u => u.Receptionist)
                                                 .Where(c => c.RoleId == 3)
                                                 .Select(s => new GetReceptionistDTO
                                                 {
                                                     Id = s.UserId,
                                                     FirstName = s.FirstName,
                                                     LastName = s.LastName,
                                                     Email = s.Email,
                                                     MobileNo = s.MobileNo,
                                                     DateOfBirth = s.DateOfBirth,
                                                     Qualification = s.Receptionist.Qualification,
                                                 }).ToArrayAsync();

            if (receptionistList == null)
            {
                throw new KeyNotFoundException("No Data found");
            }
            return receptionistList;
        }
    }
}