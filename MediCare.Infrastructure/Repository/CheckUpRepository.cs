using MediCare_MVC_Project.MediCare.Application.DTOs.CheckUpDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces;
using MediCare_MVC_Project.MediCare.Application.Interfaces.CheckUpListManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using MediCare_MVC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class CheckUpRepository : GenericRepository<PatientNote>, ICheckUpRepository
    {
        private readonly IEmailHelper _emailHelper;
        private readonly IDownloadHelper _downloadService;

        public CheckUpRepository(ApplicationDBContext context, IEmailHelper emailHelper, IDownloadHelper downloadHelper) : base(context)
        {
            _emailHelper = emailHelper;
            _downloadService = downloadHelper;
        }

        public async Task<bool> AddPatientNoteQuery(CheckUpDTO patientNoteView, int id)
        {
            if (patientNoteView == null)
                throw new Exception("Data not sufficient.");

            var newNotes = new PatientNote
            {
                AppointmentId = patientNoteView.AppointmentId,
                NoteUrl = patientNoteView.NoteText,
                CreatedBy = id,
                CreatedAt = DateTime.UtcNow
            };

            _context.PatientNotes.Add(newNotes);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task DeletePatientNotesQuery(int id)
        {
            var existingNotes = await _context.PatientNotes.FindAsync(id);

            if (existingNotes == null)
                throw new Exception("No Data found.");

            _context.PatientNotes.Remove(existingNotes);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]> DownloadNotesPdfQuery(int id)
        {
            try
            {
                var notesData = await this.GetNotesData(id);

                if (notesData == null)
                    throw new Exception("Note's data not found.");

                byte[] pdf = await _downloadService.ConvertNoteTextToPdfAsync(notesData);

                return pdf;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICollection<GetCheckUpDTO>> GetAllCheckUpQuery()
        {
            var checkUpList = await _context.PatientNotes.Include(a => a.Appointment)
                                                         .ThenInclude(a => a.Patient)
                                                         .Include(a => a.Appointment.Doctor)
                                                         .ThenInclude(a => a.User)
                                                         .Select(s => new GetCheckUpDTO
                                                         {
                                                             PatientNoteId = s.PatientNoteId,
                                                             PatientName = s.Appointment.Patient.FirstName + " " + s.Appointment.Patient.LastName,
                                                             DoctorName = s.Appointment.Doctor.User.FirstName + " " + s.Appointment.Doctor.User.LastName,
                                                             Email = s.Appointment.Patient.Email,
                                                             MobileNo = s.Appointment.Patient.MobileNo,
                                                             AppointmentDate = s.Appointment.AppointmentDate,
                                                             StartTime = s.Appointment.AppointmentStarts,
                                                             EndTime = s.Appointment.AppointmentEnds,
                                                             AppointmentDescription =s.Appointment.AppointmentDescription,
                                                             NoteText = s.NoteUrl
                                                         }).ToListAsync();
            if (checkUpList == null)
                throw new Exception("No data found.");

            return checkUpList;
        }

        public async Task<CheckUpDTO> GetCheckupFormDataQuery(int id)
        {
            var existingRecord = await _context.Appointments.Include(p => p.Patient)
                                                            .Include(p => p.PatientNote)
                                                            .Include(p => p.Doctor)
                                                            .ThenInclude(p => p.User)
                                                            .Where(c => c.AppointmentId == id)
                                                            .Select(s => new CheckUpDTO
                                                            {
                                                                AppointmentId = s.AppointmentId,
                                                                PatientName = s.Patient.FirstName + " " + s.Patient.LastName,
                                                                DoctorName = s.Doctor.User.FirstName + " " + s.Doctor.User.LastName,
                                                                AppointmentDate = s.AppointmentDate,
                                                                NoteText = s.PatientNote.NoteUrl
                                                            }).FirstOrDefaultAsync();

            if (existingRecord == null)
                throw new Exception("No data found.");

            return existingRecord;
        }

        public async Task<PdfNoteDTO> GetNotesData(int id)
        {
            var notesData = await _context.Appointments.Include(p => p.Patient)
                                                            .Include(p => p.PatientNote)
                                                            .Include(p => p.Doctor)
                                                            .ThenInclude(p => p.User)
                                                            .Where(c => c.PatientNote.PatientNoteId == id)
                                                            .Select(s => new PdfNoteDTO
                                                            {
                                                                PatientName = s.Patient.FirstName + " " + s.Patient.LastName,
                                                                DateOfBirth = s.Patient.DateOfBirth,
                                                                Email = s.Patient.Email,
                                                                Gender = s.Patient.Gender,
                                                                Address = s.Patient.Address,
                                                                AadharNo = s.Patient.AadharNo,
                                                                MobileNo = s.Patient.MobileNo,
                                                                AppointmentId = s.AppointmentId,
                                                                DoctorName = s.Doctor.User.FirstName + " " + s.Doctor.User.LastName,
                                                                AppointmentDate = s.AppointmentDate,
                                                                StartTime = s.AppointmentStarts,
                                                                EndTime = s.AppointmentEnds,
                                                                Description = s.AppointmentDescription,
                                                                NoteText = s.PatientNote.NoteUrl
                                                            }).FirstOrDefaultAsync();

            if (notesData == null)
                throw new Exception("No Data found.");

            return notesData;
        }

        public async Task<bool> SendPatientNotePdfQuery(int id)
        {
            try
            {
                var notes = await this.GetNotesData(id);
                byte[] pdf = await this.DownloadNotesPdfQuery(id);

                if (pdf == null || pdf.Length == 0)
                {
                    Console.WriteLine("PDF attachment is empty. Email sent without attachment.");
                }

                await _emailHelper.SendPatientNotesEmailAsync(notes.Email, notes, pdf);

                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}