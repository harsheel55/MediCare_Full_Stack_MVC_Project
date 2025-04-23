using MediCare_MVC_Project.MediCare.Application.DTOs.Dashboard;
using MediCare_MVC_Project.MediCare.Application.Interfaces;
using MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class DoctorDashboardRepository : GenericRepository<Appointment>, IDoctorDashboardRepository
    {
        public DoctorDashboardRepository(ApplicationDBContext context): base(context)
        {
            
        }

        public async Task<int> GetCompletedAppointmentQuery(int doctorId)
        {
            var ans = await _context.Appointments.Where(s => s.DoctorId == doctorId && s.Status == "Completed").CountAsync();

            return ans;
        }

        public async Task<int> GetPendingAppointmentQuery(int doctorId)
        {
            var total = await GetTotalAppointmentQuery(doctorId);
            var completed = await GetCompletedAppointmentQuery(doctorId);

            return (total - completed);
        }

        public async Task<int> GetTotalAppointmentQuery(int doctorId)
        {
            var total = await _context.Appointments.Where(s => s.DoctorId == doctorId).CountAsync();
            return total;
        }

        public async Task<decimal> GetTotalRevenueQuery(int doctorId)
        {
            var revenue = await _context.Appointments
                .Include(s => s.Invoice)
                .Where(s => s.DoctorId == doctorId && s.Invoice != null)
                .SumAsync(s => s.Invoice.Amount);

            return revenue;
        }

        public async Task<ICollection<DocDashboardDTO>> GetUpcomingAppointmentQuery(int doctorId)
        {
            var appointmentList = await _context.Appointments.Include(p => p.Patient)
                                                            .Where(s => s.DoctorId == doctorId && s.AppointmentDate > DateOnly.FromDateTime(DateTime.Now))
                                                            .Select(s => new DocDashboardDTO
                                                            {
                                                                PatientName = s.Patient.FirstName + " " + s.Patient.LastName,
                                                                Date = s.AppointmentDate,
                                                                Status = s.Status
                                                            }).ToListAsync();
            return appointmentList;
        }
    }
}
