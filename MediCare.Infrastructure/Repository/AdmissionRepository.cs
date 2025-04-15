using MediCare_MVC_Project.MediCare.Application.DTOs.AdmissionDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AdmissionManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class AdmissionRepository : GenericRepository<PatientAdmission>, IAdmissionRepository
    {
        public AdmissionRepository(ApplicationDBContext context) : base(context)
        {
            
        }

        public async Task AddAdmissionRecordsQuery(AdmissionDTO admission)
        {
            var existingRecords = await _context.PatientAdmissions.Include(s => s.Patient).FirstOrDefaultAsync(p => p.Patient.AadharNo == admission.AadharNo && p.AdmissionDate == admission.AdmissionDate && p.BedId == admission.BedId);

            if (existingRecords != null)
                throw new Exception("Records already exists");

            var patientRecord = await _context.Patients.FirstOrDefaultAsync(s => s.AadharNo == admission.AadharNo);

            var newRecords = new PatientAdmission
            {
                PatientId = patientRecord.PatientId,
                BedId = admission.BedId,
                AdmissionDate = admission.AdmissionDate,
                DischargeDate = admission.DischargeDate,
                IsDischarged = admission.IsDischarged,
                CreatedAt = DateTime.UtcNow
            };

            var existingBed = await _context.Beds.FindAsync(newRecords.BedId);

            existingBed.IsOccupied = true;

            _context.Beds.Update(existingBed);

            _context.PatientAdmissions.Add(newRecords);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAdmissionRecordQuery(int AdmissionId)
        {
            var existingRecord = await _context.PatientAdmissions.FindAsync(AdmissionId);
            if (existingRecord == null)
                throw new Exception("No Record found.");

            _context.PatientAdmissions.Remove(existingRecord);

            var relieveBed = await _context.Beds.FindAsync(existingRecord.BedId);
            relieveBed.IsOccupied = false;

            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<GetAdmissionDTO>> GetAllAdmissionRecordsQuery()
        {
            var recordsList = await _context.PatientAdmissions.Include(p => p.Patient)
                                                              .Include(p => p.Bed)
                                                              .ThenInclude(p => p.Room)
                                                              .Select(s => new GetAdmissionDTO
                                                              {
                                                                  AdmissionId = s.AdmissionId,
                                                                  FirstName = s.Patient.FirstName,
                                                                  LastName = s.Patient.LastName,
                                                                  MobileNo = s.Patient.MobileNo,
                                                                  Email = s.Patient.Email,
                                                                  RoomType = s.Bed.Room.RoomType,
                                                                  RoomNo = s.Bed.Room.RoomNumber,
                                                                  BedNo = s.Bed.BedNumber,
                                                                  AdmissionDate = s.AdmissionDate,
                                                                  DischargeDate = s.DischargeDate
                                                              }).ToListAsync();

            if (recordsList == null)
                throw new Exception("No Records found.");

            return recordsList;
        }
    }
}