using MediCare_MVC_Project.MediCare.Application.DTOs.AppointmentDTOs;
using MediCare_MVC_Project.MediCare.Application.Interfaces;
using MediCare_MVC_Project.MediCare.Application.Interfaces.AppointmentManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly IEmailHelper _emailHelper;
        public AppointmentRepository(ApplicationDBContext context, IEmailHelper emailHelper) : base(context)
        {
            _emailHelper = emailHelper;
        }

        public async Task BookAppointmentQuery(int id, AppointmentDTO appointment)
        {
            var existingAppointment = await _context.Appointments.FirstOrDefaultAsync(p => p.PatientId == appointment.PatientId && p.AppointmentDate == appointment.AppointmentDate && p.DoctorId == appointment.DoctorId);

            if (existingAppointment != null)
                throw new Exception("Appointment Already exists.");

            var newAppointment = new Appointment
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentStarts = appointment.AppointmentStarts,
                AppointmentEnds = appointment.AppointmentEnds,
                Status = appointment.Status,
                AppointmentDescription = appointment.AppointmentDescription,
                CreatedBy = id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Appointments.Add(newAppointment);
            await _context.SaveChangesAsync();

            var emailData = await _context.Appointments.Include(s => s.Patient)
                                                       .Include(s => s.Doctor)
                                                       .ThenInclude(u => u.User)
                                                       .Where(c => c.PatientId == appointment.PatientId)
                                                       .Select(s => new
                                                       {
                                                           Email = s.Patient.Email,
                                                           DoctorName = s.Doctor.User.FirstName + " " + s.Doctor.User.LastName
                                                       }).FirstOrDefaultAsync();

            await _emailHelper.SendAppointmentStatusEmailAsync(emailData.Email, emailData.DoctorName, newAppointment);
        }

        public async Task DeleteAppointmentByIdQuery(int id)
        {
            var existingRecord = await _context.Appointments.FindAsync(id);

            if (existingRecord == null)
                throw new KeyNotFoundException("No data found.");

            _context.Appointments.Remove(existingRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<GetAppointmentDTO>> GetAllAppointmentQuery()
        {
            var appointmentList = await _context.Appointments.Include(p => p.Patient)
                                                             .Include(d => d.Doctor)
                                                                .ThenInclude(d => d.User)
                                                             .Select(s => new GetAppointmentDTO
                                                             {
                                                                 AppointmentId = s.AppointmentId,
                                                                 FirstName = s.Patient.FirstName,
                                                                 LastName = s.Patient.LastName,
                                                                 DoctorName = s.Doctor.User.FirstName + " " + s.Doctor.User.LastName,
                                                                 Email = s.Patient.Email,
                                                                 AadharNo = s.Patient.AadharNo,
                                                                 AppointmentDate = s.AppointmentDate,
                                                                 AppointmentStarts = s.AppointmentStarts,
                                                                 AppointmentEnds = s.AppointmentEnds, 
                                                                 Status = s.Status,
                                                                 AppointmentDescription = s.AppointmentDescription

                                                             }).ToListAsync();

            if (appointmentList == null)
                throw new KeyNotFoundException("No Data found.");

            return appointmentList;
        }

        public async Task<GetAppointmentDTO> GetAppointmentByIdQuery(int id)
        {
            var appointment = await _context.Appointments.Include(p => p.Patient)
                                                             .Include(d => d.Doctor)
                                                                .ThenInclude(d => d.User)
                                                             .Where(c => c.AppointmentId == id)
                                                             .Select(s => new GetAppointmentDTO
                                                             {
                                                                 AppointmentId = s.AppointmentId,
                                                                 FirstName = s.Patient.FirstName,
                                                                 LastName = s.Patient.LastName,
                                                                 DoctorName = s.Doctor.User.FirstName + " " + s.Doctor.User.LastName,
                                                                 Email = s.Patient.Email,
                                                                 AadharNo = s.Patient.AadharNo,
                                                                 AppointmentDate = s.AppointmentDate,
                                                                 AppointmentStarts = s.AppointmentStarts,
                                                                 AppointmentEnds = s.AppointmentEnds,
                                                                 Status = s.Status,
                                                                 AppointmentDescription = s.AppointmentDescription

                                                             }).FirstOrDefaultAsync();

            if (appointment == null)
                throw new KeyNotFoundException("No Data found.");

            return appointment;
        }

        public async Task SendReminderEmailQuery(int id)
        {
            var existingAppointment = await this.GetAppointmentByIdQuery(id);
            await _emailHelper.SendAppointmentReminderEmailAsync(existingAppointment.Email, existingAppointment.DoctorName, existingAppointment);
        }
    }
}
