using MediCare_MVC_Project.MediCare.Application.DTOs.Dashboard;
using MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class ReceptionistDashboardRepository : GenericRepository<Receptionist>, IReceptionistDashboardRepository
    {
        public ReceptionistDashboardRepository(ApplicationDBContext context) : base(context)
        {
            
        }

        public async Task<ICollection<ReceptionistDashDTO>> GetRecentAppointmentQuery()
        {
            var appointmentList = await _context.Appointments.Include(p => p.Patient)
                                                              .Include(d => d.Doctor)
                                                              .ThenInclude(d => d.User)
                                                            .Where(s => s.AppointmentDate < DateOnly.FromDateTime(DateTime.Now))
                                                            .Select(s => new ReceptionistDashDTO
                                                            {
                                                                PatientName = s.Patient.FirstName + " " + s.Patient.LastName,   
                                                                DoctorName = s.Doctor.User.FirstName + " " + s.Doctor.User.LastName,
                                                                AppointmentDate = s.AppointmentDate,
                                                                Status = s.Status
                                                            }).ToListAsync();
            return appointmentList;
        }

        public async Task<int> GetTodaysAppointmentCountQuery()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var count = await _context.Appointments.Where(s => s.AppointmentDate == today).CountAsync();

            return count;
        }

        public async Task<int> GetTotalAppointmentCountQuery()
        {
            var count = await _context.Appointments.CountAsync();
            return count;
        }

        public async Task<int> GetTotalPatientCountQuery()
        {
            var count = await _context.Patients.CountAsync();
            return count;
        }

        public async Task<decimal> GetTotalRevenueQuery()
        {
            var count = await _context.Invoices.SumAsync(s => s.Amount);
            return count;
        }

        public async Task<ICollection<ReceptionistUpcomingDTO>> GetUpcomingAppointmentQuery()
        {
            var appointmentList = await _context.Appointments.Include(p => p.Patient)
                                                             .Where(s => s.AppointmentDate > DateOnly.FromDateTime(DateTime.Now))
                                                             .Select(s => new ReceptionistUpcomingDTO
                                                             {
                                                                PatientName = s.Patient.FirstName + " " + s.Patient.LastName,
                                                                AppointmentDate = s.AppointmentDate,
                                                              }).ToListAsync();
            return appointmentList;
        }
    }
}
