using System.Numerics;
using System.Runtime.InteropServices;
using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs.DoctorDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.DoctorManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly IMapper _mapper;
        public DoctorRepository(ApplicationDBContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;   
        }

        public async Task AddDoctorQuery(int id, UserDoctorDTO doctor)
        {
            try
            {
                // Check if user exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == doctor.Id && u.RoleId == 2);

                if (existingUser == null)
                {
                    throw new ArgumentException("Doctor not registered as a user.");
                }

                // Check if doctor record already exists
                var doctorExists = await _context.Doctors.AnyAsync(d => d.UserId == doctor.Id);
                if (doctorExists)
                {
                    throw new ArgumentException("Doctor profile already exists.");
                }

                var user = _mapper.Map<User>(doctor);
                var newDoctor = _mapper.Map<Doctor>(doctor); // This will auto-set UserId from dto.Id

                _context.Doctors.Add(newDoctor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add doctor: " + ex.Message);
            }
        }

        public async Task<ICollection<GetDoctorDTO>> GetAllDoctorQuery()
        {
            var doctorList = await _context.Users.Include(u => u.Doctor)
                                                 .Where(c => c.RoleId == 2)
                                                 .Select(s => new GetDoctorDTO
                                                 {
                                                     Id = s.UserId,
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
