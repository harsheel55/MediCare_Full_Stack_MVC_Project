using MediCare_MVC_Project.MediCare.Application.DTOs.LabTestManagement;
using MediCare_MVC_Project.MediCare.Application.Interfaces.LabTestManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class PatientTestRepository : GenericRepository<PatientTest>, IPatientTestRepository
    {
        public PatientTestRepository(ApplicationDBContext context) : base(context)
        {
            
        }
        public async Task AddPatientTestQuery(PatientTestDTO patientTest)
        {
            var existingRecords = await _context.PatientTests.FirstOrDefaultAsync(s => s.TestId == patientTest.TestId && s.TestDate == s.TestDate && s.Patient.AadharNo == patientTest.AadharNo);

            if (existingRecords != null)
                throw new Exception("Test data already exists.");

            var patientRecord = await _context.Patients.FirstOrDefaultAsync(s => s.AadharNo == patientTest.AadharNo);

            if (patientRecord == null)
                throw new Exception("No patient record found.");

            var newTestRecord = new PatientTest
            {
                PatientId = patientRecord.PatientId,
                TestId = patientTest.TestId,
                TestDate = patientTest.TestDate,
                Result = patientTest.Result
            };

            // Optionally mark the appointment as completed
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.PatientId == patientRecord.PatientId);

            appointment.Status = "Completed";
            appointment.UpdatedAt = DateTime.UtcNow;

            _context.Appointments.Update(appointment);
            _context.PatientTests.Add(newTestRecord);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatientTestQuery(int patientTestId)
        {
            var existingRecords = await _context.PatientTests.FindAsync(patientTestId);

            if (existingRecords == null)
                throw new Exception("No Record Found.");

            _context.PatientTests.Remove(existingRecords);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<GetPatientTestDTO>> GetAllPatientTestQuery()
        {
            var testList = await _context.PatientTests.Include(p => p.Patient)
                                                        .Include(p => p.LabTest)
                                                          .Select(s => new GetPatientTestDTO
                                                          {
                                                            PatientTestId = s.PatientTestId,
                                                            FirstName = s.Patient.FirstName,
                                                            LastName = s.Patient.LastName,
                                                            TestName = s.LabTest.TestName,
                                                            TestDate = s.TestDate,
                                                            Cost = s.LabTest.Cost,
                                                            Result = s.Result
                                                          }).ToListAsync();

            if (testList == null)
                throw new Exception("No records found.");

            return testList;
        }

        public async Task<ICollection<GetPatientTestDTO>> GetPatientTestByDoctorQuery(int doctorId)
        {
            var patientTestList = await _context.Appointments.Where(a => a.DoctorId == doctorId)
                                                             .Include(a => a.Patient)
                                                                 .ThenInclude(p => p.PatientTests)
                                                                     .ThenInclude(pt => pt.LabTest)
                                                             .SelectMany(a => a.Patient.PatientTests.Select(pt => new GetPatientTestDTO
                                                             {
                                                                 PatientTestId = pt.PatientTestId,
                                                                 FirstName = a.Patient.FirstName,
                                                                 LastName = a.Patient.LastName,
                                                                 TestName = pt.LabTest.TestName,
                                                                 TestDate = pt.TestDate,
                                                                 Cost = pt.LabTest.Cost,
                                                                 Result = pt.Result
                                                             }))
                                                             .ToListAsync();
            if (patientTestList == null)
                throw new Exception("No Patient Test found.");

            return patientTestList;
        }

        public async Task UpdatePatientTestQuery(int patientTestId, DateOnly date, string result)
        {
            var existingRecords = await _context.PatientTests.FindAsync(patientTestId);

            if (existingRecords == null)
                throw new Exception("No Record found.");

            existingRecords.TestDate = date;
            existingRecords.Result = result;

            _context.PatientTests.Update(existingRecords);
            await _context.SaveChangesAsync();
        }
    }
}