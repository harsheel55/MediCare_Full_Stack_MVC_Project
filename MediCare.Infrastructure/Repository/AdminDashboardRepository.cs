using MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement;
using MediCare_MVC_Project.MediCare.Domain.Entity;
using MediCare_MVC_Project.MediCare.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public class AdminDashboardRepository : GenericRepository<User>, IAdminDashboardRepository
    {
        public AdminDashboardRepository(ApplicationDBContext context) : base(context)
        {
            
        }

        public async Task<int> GetAvailableBedsCountQuery()
        {
            return await _context.Beds.Where(s => s.IsOccupied == false).CountAsync();
        }
        public async Task<List<object>> GetRecentAppointmentsQuery()
        {
            var recentAppointments = await _context.Appointments
                .Include(s => s.Doctor).ThenInclude(s => s.User)
                .Include(s => s.Patient)
                .OrderByDescending(a => a.AppointmentDate) // Get the most recent ones first
                .Take(10)
                .Select(a => new
                {
                    a.AppointmentId,
                    PatientName = a.Patient.FirstName + " " + a.Patient.LastName, // Assuming a Patient relation exists
                    DoctorName = a.Doctor.User.FirstName + " " + a.Doctor.User.LastName, // Assuming a Doctor relation exists
                    a.AppointmentDate,
                    a.Status
                })
                .ToListAsync();

            return recentAppointments.Cast<object>().ToList(); // Cast to List<object>
        }



        public Task<Dictionary<string, decimal>> GetRevenueBreakdownQuery()
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTodayAppointmentsCountQuery()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var count = await _context.Appointments
                .Where(a => a.AppointmentDate == today)
                .CountAsync();

            return count;
        }

        public Task<List<dynamic>> GetTopPerformingDoctorsQuery()
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalAppointmentsQuery()
        {
            return await _context.Appointments.CountAsync();
        }

        public async Task<int> GetTotalDoctorsQuery()
        {
            return await _context.Doctors.CountAsync();
        }

        public async Task<int> GetTotalLabTestsQuery()
        {
            return await _context.LabTests.CountAsync();
        }

        public async Task<int> GetTotalPatientsQuery()
        {
            return await _context.Patients.CountAsync();
        }

        public async Task<int> GetTotalReceptionistsQuery()
        {
            return await _context.Receptionists.CountAsync();
        }

        public Task<decimal> GetTotalRevenueQuery()
        {
            throw new NotImplementedException();
        }

        public async Task<List<int>> GetWeeklyAppointmentsQuery()
        {
            DateOnly startOfWeek = DateOnly.FromDateTime(DateTime.Now.StartOfWeek());
            DateOnly endOfWeek = DateOnly.FromDateTime(DateTime.Now.EndOfWeek());

            var result = await _context.Appointments
                .Where(a => a.AppointmentDate >= startOfWeek && a.AppointmentDate <= endOfWeek)
                .GroupBy(a => a.AppointmentDate.DayOfWeek)
                .OrderBy(g => g.Key)
                .Select(g => g.Count())
                .ToListAsync();

            return result;
        }
    }
}