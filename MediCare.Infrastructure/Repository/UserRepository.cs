using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces;
using MediCare_MVC_Project.MediCare.Application.Interfaces.UserManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IEmailHelper _emailService;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDBContext context, IEmailHelper emailService, IMapper mapper) : base(context)
        {
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task AddUserQuery(int id, UserRegisterDTO userDto)
        {
            try
            {
                // Check if user already exists in the database
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
                if (existingUser != null)
                {
                    throw new ArgumentException("User already exists.");
                }

                if (userDto.RoleId != 1)
                    throw new ArgumentException("User only for Admin");

                // Hash the password
                var passwordHasher = new PasswordHasher<UserRegisterDTO>();
                var hashedPassword = passwordHasher.HashPassword(userDto, userDto.Password);

                // Create new user object
                var newUser = new User
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    DateOfBirth = userDto.DateOfBirth,
                    DateOfJoining = userDto.DateOfJoining,
                    DateOfRelieving = userDto.DateOfRelieving,
                    MobileNo = userDto.MobileNo,
                    EmergencyNo = userDto.EmergencyNo,
                    Password = hashedPassword,
                    Active = userDto.Active,
                    RoleId = userDto.RoleId,
                    CreatedBy = id, // No need to convert since it's already an int
                    CreatedAt = DateTime.UtcNow
                };

                // Add user to database
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                // Send email with login credentials
                await _emailService.SendUserRegistrationEmailAsync(userDto.Email, userDto.Password);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

        public async Task AddDoctorQuery(int id, UserDoctorDTO userDoctor)
        {
            try
            {
                // Check specialization is present in Specialization Table or not
                var existingSpecialization = await _context.Specializations.FindAsync(userDoctor.SpecializationId);
                var existingUser = await _context.Users.FirstOrDefaultAsync(s => s.Email == userDoctor.Email);

                if (existingSpecialization == null)
                    throw new ArgumentException("Specialization is not valid");

                if (userDoctor.RoleId != 2)
                    throw new ArgumentException("Role id is not valid");

                if (existingUser != null)
                    throw new ArgumentException("Doctor already Exists.");

                var passwordHasher = new PasswordHasher<UserDoctorDTO>();
                var hashedPassword = passwordHasher.HashPassword(userDoctor, userDoctor.Password);

                // Seed User table related data from the UserDoctorDTO
                var user = _mapper.Map<User>(userDoctor);

                user.RoleId = 2;
                user.Password = hashedPassword;
                user.CreatedBy = id;

                var doctor = _mapper.Map<Doctor>(userDoctor);

                doctor.User = user;
                doctor.SpecializationId = userDoctor.SpecializationId;
                doctor.Qualification = userDoctor.Qualification;
                doctor.LicenseNumber = userDoctor.LicenseNumber;

                // Add docter related data into doctor table
                await _context.Doctors.AddAsync(doctor);
                await _context.SaveChangesAsync();

                await _emailService.SendUserRegistrationEmailAsync(userDoctor.Email, userDoctor.Password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddReceptionistQuery(int id, UserReceptionistDTO userReceptionist)
        {
            try
            {
                var existingReceptionist = await _context.Users.FirstOrDefaultAsync(s => s.Email == userReceptionist.Email);

                if (existingReceptionist != null)
                    throw new ArgumentException("Receptionist is already exist.");

                if (userReceptionist.RoleId != 3)
                    throw new ArgumentException("Role id is not valid");

                var passwordHasher = new PasswordHasher<UserReceptionistDTO>();
                var hashedPassword = passwordHasher.HashPassword(userReceptionist, userReceptionist.Password);

                // Seed User table related data from the UserDoctorDTO
                var user = _mapper.Map<User>(userReceptionist);

                user.RoleId = 3;
                user.Password = hashedPassword;
                user.CreatedBy = id;

                var receptionist = _mapper.Map<Receptionist>(userReceptionist);

                receptionist.User = user;
                receptionist.Qualification = userReceptionist.Qualification;

                // Add docter related data into doctor table
                await _context.Receptionists.AddAsync(receptionist);
                await _context.SaveChangesAsync();

                await _emailService.SendUserRegistrationEmailAsync(userReceptionist.Email, userReceptionist.Password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteUserQuery(string email)
        {
            try
            {
                var entity = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (entity == null)
                    throw new Exception("User Not found.");

                if (entity.RoleId == 2)
                {
                    var doctorId = await _context.Doctors.Where(d => d.User.Email == email).FirstOrDefaultAsync();

                    if (doctorId != null)
                        _context.Doctors.Remove(doctorId);
                }

                if (entity.RoleId == 3)
                {
                    var receptionistId = await _context.Receptionists.Where(d => d.User.Email == email).FirstOrDefaultAsync();

                    if (receptionistId != null)
                        _context.Receptionists.Remove(receptionistId);
                }

                _context.Users.Remove(entity);  // Hard Delete

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task<ICollection<UserDTO>> GetAllUsersQuery()
        {
            try
            {
                var userList = await _context.Users
                             .Select(s => new UserDTO
                             {
                                 UserId = s.UserId,
                                 FirstName = s.FirstName,
                                 LastName = s.LastName,
                                 Email = s.Email,
                                 MobileNo = s.MobileNo,
                                 EmergencyNo = s.EmergencyNo,
                                 DateOfBirth = s.DateOfBirth,
                                 Role = s.Role.RoleName,
                                 Status = s.Active
                             }).ToListAsync();

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDTO> GetUserByIdQuery(int id)
        {
            try
            {
                var user = await _context.Users.Where(s => s.UserId == id)
                                             .Select(s => new UserDTO
                                             {
                                                 UserId = s.UserId,
                                                 FirstName = s.FirstName,
                                                 LastName = s.LastName,
                                                 Email = s.Email,
                                                 MobileNo = s.MobileNo,
                                                 EmergencyNo = s.EmergencyNo,
                                                 DateOfBirth = s.DateOfBirth,
                                                 Role = s.Role.RoleName,
                                                 Status = s.Active
                                             }).FirstOrDefaultAsync();

                if (user == null)
                    throw new KeyNotFoundException($"No User found with {id}");

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateUserQuery(int updatedById, int id, UserRegisterDTO user)
        {
            try
            {
                // Fetch the existing user
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                {
                    throw new KeyNotFoundException($"User with ID {id} not found.");
                }

                // Update allowed fields
                existingUser.FirstName = user.FirstName ?? existingUser.FirstName;
                existingUser.LastName = user.LastName ?? existingUser.LastName;
                existingUser.Email = user.Email ?? existingUser.Email;
                existingUser.MobileNo = user.MobileNo ?? existingUser.MobileNo;
                existingUser.EmergencyNo = user.EmergencyNo ?? existingUser.EmergencyNo;
                existingUser.DateOfBirth = user.DateOfBirth;
                existingUser.RoleId = user.RoleId;
                existingUser.Active = user.Active;
                existingUser.UpdatedBy = updatedById;
                existingUser.UpdatedAt = DateTime.UtcNow;

                // Only update password if provided
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var passwordHasher = new PasswordHasher<User>();
                    existingUser.Password = passwordHasher.HashPassword(existingUser, user.Password);
                }

                _context.Users.Update(existingUser);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task<int> GetDoctorsIdQuery(int userId)
        {
            var id = await _context.Users.Include(d => d.Doctor)
                                         .Where(c => c.UserId == userId)
                                         .Select(s => s.Doctor.DoctorId).FirstOrDefaultAsync();
            if (id == 0)
                throw new Exception("No Doctor found.");
            return id;
        }
    }
}