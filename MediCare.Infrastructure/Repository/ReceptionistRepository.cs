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
                newUser.Active = receptionist.Active;

                // Save new user to get the generated UserId
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // STEP 2: Use newUser.UserId in Receptionist
                receptionist.UserId = newUser.UserId;

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
                                                     UserId = s.UserId,
                                                     FirstName = s.FirstName,
                                                     LastName = s.LastName,
                                                     Email = s.Email,
                                                     MobileNo = s.MobileNo,
                                                     DateOfBirth = s.DateOfBirth,
                                                     Status = s.Active,
                                                     Qualification = s.Receptionist.Qualification,
                                                 }).ToArrayAsync();

            if (receptionistList == null)
            {
                throw new KeyNotFoundException("No Data found");
            }
            return receptionistList;
        }

        public async Task<UserReceptionistDTO> GetReceptionistByIdQuery(int id)
        {
            var existingReceptionist = await _context.Receptionists.Include(s => s.User)
                                                                   .Where(c => c.UserId == id)
                                                                   .Select(s => new UserReceptionistDTO
                                                                   {
                                                                       UserId = s.UserId,
                                                                       FirstName = s.User.FirstName,
                                                                       LastName = s.User.LastName,
                                                                       MobileNo = s.User.MobileNo,
                                                                       Email = s.User.Email,
                                                                       EmergencyNo = s.User.EmergencyNo,
                                                                       DateOfBirth = s.User.DateOfBirth,
                                                                       DateOfJoining = s.User.DateOfJoining,
                                                                       DateOfRelieving = s.User.DateOfRelieving,
                                                                       Password = s.User.Password,
                                                                       RoleId = s.User.RoleId,
                                                                       Active = s.User.Active,
                                                                       Qualification = s.Qualification
                                                                   }).FirstOrDefaultAsync();

            if (existingReceptionist == null)
                throw new Exception("No receptionist found.");

            return existingReceptionist;
        }

        public async Task UpdateReceptionistQuery(int id, UserReceptionistDTO receptionistDto, int updatedById)
        {
            try
            {
                if (receptionistDto == null)
                    throw new ArgumentNullException(nameof(receptionistDto), "Receptionist object is null.");

                if (string.IsNullOrWhiteSpace(receptionistDto.Email))
                    throw new ArgumentException("Email is required.");

                if (receptionistDto.RoleId <= 0)
                    throw new ArgumentException("Invalid RoleId.");

                // STEP 1: Find existing user by UserId
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                if (existingUser == null)
                    throw new ArgumentException("User not found.");

                // STEP 2: Check for duplicate email (excluding current user)
                var emailConflict = await _context.Users
                    .AnyAsync(u => u.Email == receptionistDto.Email && u.UserId != id);
                if (emailConflict)
                    throw new ArgumentException("Email already in use by another user.");

                // STEP 3: Update User fields
                existingUser.FirstName = receptionistDto.FirstName;
                existingUser.LastName = receptionistDto.LastName;
                existingUser.Email = receptionistDto.Email;
                existingUser.MobileNo = receptionistDto.MobileNo;
                existingUser.RoleId = receptionistDto.RoleId;
                existingUser.Active = receptionistDto.Active;
                existingUser.UpdatedBy = updatedById;
                existingUser.UpdatedAt = DateTime.UtcNow;

                // Optionally update password if provided
                if (!string.IsNullOrWhiteSpace(receptionistDto.Password))
                {
                    var passwordHasher = new PasswordHasher<UserReceptionistDTO>();
                    existingUser.Password = passwordHasher.HashPassword(receptionistDto, receptionistDto.Password);
                }

                // STEP 4: Update Receptionist entity
                var existingReceptionist = await _context.Receptionists
                    .FirstOrDefaultAsync(r => r.UserId == id);

                if (existingReceptionist == null)
                    throw new ArgumentException("Receptionist record not found.");

                existingReceptionist.Qualification = receptionistDto.Qualification;
                existingReceptionist.UpdatedAt = DateTime.UtcNow;

                // Save changes
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update receptionist: " + ex.Message);
            }
        }
    }
}