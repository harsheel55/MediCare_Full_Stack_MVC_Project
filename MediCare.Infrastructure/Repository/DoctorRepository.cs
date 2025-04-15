using System.Numerics;
using System.Runtime.InteropServices;
using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs.DoctorDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces;
using MediCare_MVC_Project.MediCare.Application.Interfaces.DoctorManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly IMapper _mapper;
        private readonly IEmailHelper _emailHelper;

        public DoctorRepository(ApplicationDBContext context, IMapper mapper, IEmailHelper emailHelper) : base(context)
        {
            _mapper = mapper;
            _emailHelper = emailHelper;
        }

        public async Task AddDoctorQuery(int id, UserDoctorDTO doctor)
        {
            try
            {
                if (doctor == null)
                    throw new ArgumentNullException(nameof(doctor), "Doctor object is null.");

                if (string.IsNullOrWhiteSpace(doctor.Email))
                    throw new ArgumentException("Email is required.");

                if (doctor.RoleId <= 0)
                    throw new ArgumentException("Invalid RoleId.");

                // Check if user exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == doctor.Email && u.RoleId == doctor.RoleId);

                if (existingUser != null)
                {
                    throw new ArgumentException("Doctor already Exists.");
                }

                var newUser = _mapper.Map<User>(doctor);

                // Hash the password
                var passwordHasher = new PasswordHasher<UserDoctorDTO>();
                var hashedPassword = passwordHasher.HashPassword(doctor, doctor.Password);

                newUser.Password = hashedPassword;
                newUser.RoleId = doctor.RoleId;
                newUser.CreatedBy = id;

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                doctor.Id = newUser.UserId;

                var newDoctor = _mapper.Map<Doctor>(doctor);
                newDoctor.CreatedAt = DateTime.UtcNow;
                newDoctor.UserId = newUser.UserId;

                newDoctor.CreatedAt = DateTime.UtcNow;

                _context.Doctors.Add(newDoctor);
                await _context.SaveChangesAsync();

                await _emailHelper.SendUserRegistrationEmailAsync(doctor.Email, doctor.Password);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add doctor: " + ex.Message);
            }
        }

        public async Task DeleteDoctorQuery(string email)
        {
            // First find the doctor that uses the user
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.User.Email == email);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync(); 
            }

            // Then delete the user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task<ICollection<GetDoctorDTO>> GetAllDoctorQuery()
        {
            var doctorList = await _context.Users.Include(u => u.Doctor)
                                                 .Where(c => c.RoleId == 2)
                                                 .Select(s => new GetDoctorDTO
                                                 {
                                                     Id = s.UserId,
                                                     DoctorId = s.Doctor.DoctorId,
                                                     FirstName = s.FirstName,
                                                     LastName = s.LastName,
                                                     Email = s.Email,
                                                     MobileNo = s.MobileNo,
                                                     DateOfBirth = s.DateOfBirth,
                                                     Specialization = s.Doctor.Specialization.SpecializationName,
                                                     Qualification = s.Doctor.Qualification,
                                                     LicenseNumber = s.Doctor.LicenseNumber
                                                 }).ToArrayAsync();

            if(doctorList == null)
            {
                throw new KeyNotFoundException("No Data found");
            }
            return doctorList;
        }
    }
}
