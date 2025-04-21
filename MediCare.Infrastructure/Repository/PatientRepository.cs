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

        public async Task DeletePatientQuery(int id)
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

        public async Task<GetPatientDTO> GetPatientByIdQuery(int id)
        {
            var existingPatient = await _context.Patients.Where(s => s.PatientId == id)
                                                         .Select(s => new GetPatientDTO
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
                                                         }).FirstOrDefaultAsync();

            if (existingPatient == null)
                throw new Exception("No Patient found.");

            return existingPatient;
        }

        public async Task<ICollection<GetPatientDTO>> GetPatientsByDoctorIdQuery(int doctorId)
        {
            var patientList = await _context.Appointments.Include(s => s.Patient)
                                                     .Where(s => s.DoctorId == doctorId)
                                                     .Select(s => new GetPatientDTO
                                                     {
                                                         PatientId = s.PatientId,
                                                         FirstName = s.Patient.FirstName,
                                                         LastName = s.Patient.LastName,
                                                         Email = s.Patient.Email,
                                                         DateOfBirth = s.Patient.DateOfBirth,
                                                         Gender = s.Patient.Gender,
                                                         AadharNo = s.Patient.AadharNo,
                                                         Address = s.Patient.Address,
                                                         City = s.Patient.City,
                                                         MobileNo = s.Patient.MobileNo,
                                                         Active = s.Patient.Active
                                                     }).ToListAsync();
            if (patientList == null)
                throw new Exception("No patient found.");
            return patientList;
        }

        public async Task UpdatePatientQuery(int id, GetPatientDTO getPatient, int updatedBy)
        {
            var existingPatient = await this.GetByIdAsync(id);

            if (existingPatient == null)
                throw new Exception($"No Patient found with {id}.");

            existingPatient.FirstName = getPatient.FirstName;
            existingPatient.LastName = getPatient.LastName;
            existingPatient.DateOfBirth = getPatient.DateOfBirth;
            existingPatient.AadharNo = getPatient.AadharNo;
            existingPatient.MobileNo = getPatient.MobileNo;
            existingPatient.Gender = getPatient.Gender;
            existingPatient.Address = getPatient.Address;
            existingPatient.City = getPatient.City;
            existingPatient.Email = getPatient.Email;
            existingPatient.Active = getPatient.Active;
            existingPatient.UpdatedBy = updatedBy;
            existingPatient.UpdatedAt = DateTime.UtcNow;

            _context.Patients.Update(existingPatient);
            await _context.SaveChangesAsync();
        }
    }
}