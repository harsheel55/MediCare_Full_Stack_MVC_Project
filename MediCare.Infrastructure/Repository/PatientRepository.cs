using MediCare_MVC_Project.MediCare.Application.DTOs.PatientDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces;
using MediCare_MVC_Project.MediCare.Application.Interfaces.PatientManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly IEmailHelper _emailHelper;

        public PatientRepository(ApplicationDBContext context, IEmailHelper emailHelper) : base(context)
        {
            _emailHelper = emailHelper; 
        }

        public async Task AddPatientQuery(int id, GetPatientDTO patient)
        {
            var existingPatient = await _context.Patients.FirstOrDefaultAsync(s => s.AadharNo == patient.AadharNo || s.Email == patient.Email);

            if (existingPatient != null)
                throw new Exception("Patient already exists.");

            var newPatient = new Patient
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                Gender = patient.Gender,
                MobileNo = patient.MobileNo,
                DateOfBirth = patient.DateOfBirth,
                AadharNo = patient.AadharNo,
                Address = patient.Address,
                City = patient.City,
                Active = patient.Active,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = id
            };

            _context.Patients.Add(newPatient);
            await _context.SaveChangesAsync();

            await _emailHelper.SendPatientRegistrationEmailAsync(patient);
        }

        public async Task DeletePatientAsync(int id)
        {
            var existingPatient = await _context.Patients.FindAsync(id);

            if (existingPatient == null)
                throw new KeyNotFoundException("No Patient found.");

            _context.Patients.Remove(existingPatient);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<GetPatientDTO>> GetAllPatientQuery()
        {
            var patientList = await _context.Patients.Select(s => new GetPatientDTO
            {
                PatientId = s.PatientId,
                FirstName = s.FirstName,
                LastName = s.LastName, 
                Email = s.Email,
                DateOfBirth = s.DateOfBirth,
                Gender = s.Gender,
                AadharNo = s.AadharNo,
                Address = s.Address, 
                City = s.City, 
                MobileNo = s.MobileNo, 
                Active = s.Active
            }).ToListAsync();

            if (patientList == null)
                throw new KeyNotFoundException("No Data found.");

            return patientList;
        }
    }
}
