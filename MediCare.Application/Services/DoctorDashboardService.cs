using MediCare_MVC_Project.MediCare.Application.DTOs.Dashboard;
using MediCare_MVC_Project.MediCare.Application.Interfaces.DashboardManagement;

namespace MediCare_MVC_Project.MediCare.Application.Services
{
    public class DoctorDashboardService : IDoctorDashboardService
    {
        private readonly IDoctorDashboardRepository _doctorDashboardRepository;

        public DoctorDashboardService(IDoctorDashboardRepository doctorDashboardRepository)
        {
            _doctorDashboardRepository = doctorDashboardRepository;
        }

        public async Task<int> GetCompletedAppointmentAsync(int doctorId)
        {
            return await _doctorDashboardRepository.GetCompletedAppointmentQuery(doctorId);
        }

        public async Task<int> GetPendingAppointmentAsync(int doctorId)
        {
            return await _doctorDashboardRepository.GetPendingAppointmentQuery(doctorId);
        }

        public async Task<int> GetTotalAppointmentAsync(int doctorId)
        {
            return await _doctorDashboardRepository.GetTotalAppointmentQuery(doctorId);
        }

        public async Task<decimal> GetTotalRevenueAsync(int doctorId)
        {
            return await _doctorDashboardRepository.GetTotalRevenueQuery(doctorId);
        }

        public async Task<ICollection<DocDashboardDTO>> GetUpcomingAppointmentAsync(int doctorId)
        {
            return await _doctorDashboardRepository.GetUpcomingAppointmentQuery(doctorId);
        }
    }
}
