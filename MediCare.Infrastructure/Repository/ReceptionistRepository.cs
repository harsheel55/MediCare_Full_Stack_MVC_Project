using AutoMapper;
using MediCare_MVC_Project.MediCare.Application.DTOs.DoctorDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.ReceptionistDTOs;
using MediCare_MVC_Project.MediCare.Application.DTOs.UserDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.ReceptionistManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class ReceptionistRepository : GenericRepository<Receptionist>, IReceptionistRepository
    {
        private readonly IMapper _mapper;

        public ReceptionistRepository(ApplicationDBContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public Task AddReceptionistQuery(int id, UserReceptionistDTO receptionist)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetReceptionistDTO>> GetAllReceptionistQuery()
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
                                                     Specialization = s.Doctor.Specialization.SpecializationName,
                                                     Qualification = s.Doctor.Qualification,
                                                     LicenseNumber = s.Doctor.LicenseNumber
                                                 }).ToArrayAsync();

            if (doctorList == null)
            {
                throw new KeyNotFoundException("No Data found");
            }
            return doctorList;
        }
    }
}
